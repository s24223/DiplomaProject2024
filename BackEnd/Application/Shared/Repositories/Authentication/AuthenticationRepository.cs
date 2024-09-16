using Domain.Repositories.ExceptionMessage;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Application.Shared.Repositories.Authentication
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        //Values
        private readonly IConfiguration _configuration;
        private readonly IExceptionMessageRepository _exceptionRepository;

        private readonly string _issuer;
        private readonly string _audience;
        private readonly string _secret;

        private readonly int _iterationCountOfHashPassword = 10000;
        private readonly int _timeInMinutesValidJWT = 10;
        private readonly int _timeInHourValidRefreshToken = 48;

        //Constructor
        public AuthenticationRepository(
            IConfiguration configuration,
            IExceptionMessageRepository exceptionRepository
            )
        {
            _configuration = configuration;
            _exceptionRepository = exceptionRepository;

            var jwtSection = _configuration.GetSection("JwtData");

            var issuer = jwtSection["Issuer"];
            var audience = jwtSection["Audience"];
            var secret = jwtSection["Secret"];

            if (string.IsNullOrWhiteSpace(issuer))
            {
                var message = _exceptionRepository.GenerateExceptionMessage
                    (
                    Messages.NotConfiguredIssuer,
                    GetType(),
                    null
                    );
                throw new NotImplementedException(message);
            }
            if (string.IsNullOrWhiteSpace(audience))
            {
                var message = _exceptionRepository.GenerateExceptionMessage
                    (
                    Messages.NotConfiguredAudience,
                    GetType(),
                    null
                    );
                throw new NotImplementedException(message);
            }
            if (string.IsNullOrWhiteSpace(secret))
            {
                var message = _exceptionRepository.GenerateExceptionMessage
                    (
                    Messages.NotConfiguredSecret,
                    GetType(),
                    null
                    );
                throw new NotImplementedException(message);
            }

            _issuer = issuer;
            _audience = audience;
            _secret = secret;
        }

        //Methods
        //Password Part
        public string GenerateSalt()
        {
            //Generate a 128-bit salt using secure PRNG
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return Convert.ToBase64String(salt);
        }

        public string HashPassword(string password, string salt)
        {
            //Password base key derivation function [Standard] - Pbkdf2
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
               password: password,
               salt: Convert.FromBase64String(salt),
               prf: KeyDerivationPrf.HMACSHA1,
               iterationCount: _iterationCountOfHashPassword,
               numBytesRequested: 256 / 8
               ));
        }

        //Jwt Part
        //Generators
        public string GenerateRefreshToken()
        {
            //Generate a 128-bit RefreshToken using secure PRNG
            byte[] salt = new byte[1024];
            using (var genNum = RandomNumberGenerator.Create())
            {
                genNum.GetBytes(salt);
            }
            return Convert.ToBase64String(salt);
        }

        public (string, DateTime) GenerateJWTStringAndDateTimeValidTo
            (
            string name,
            IEnumerable<string> roles
            )
        {
            var claims = GenerateClaims(name, roles);
            var token = GenerateJWT(claims);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            var validTo = token.ValidTo.ToLocalTime();
            return (tokenString, validTo);
        }

        //Validation
        public bool IsJWTGeneratedByThisServer(string jwt)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));

            try
            {
                tokenHandler.ValidateToken(jwt, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = _issuer,
                    ValidAudience = _audience,
                    IssuerSigningKey = secretKey,
                }, out SecurityToken validatedToken);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool IsJWTGeneratedByThisServerAndNotExpired(string jwt)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));

            try
            {
                tokenHandler.ValidateToken(jwt, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = _issuer,
                    ValidAudience = _audience,
                    IssuerSigningKey = secretKey,
                }, out SecurityToken validatedToken);

                return true;
            }
            catch
            {
                return false;
            }
        }

        //Getters
        public IEnumerable<Claim> GetClaimsFromJWT(string jwt)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(jwt);
            var claims = jwtToken.Claims.ToList();
            return claims;
        }

        public string GetNameFromClaims(IEnumerable<Claim> claims)
        {
            var name = "";
            foreach (var claim in claims)
            {
                if (claim.Type == ClaimTypes.Name)
                {
                    name = claim.Value;
                    break;
                }
            }
            return name;
        }

        public int GetTimeInHourValidRefreshToken() => _timeInHourValidRefreshToken;

        //================================================================================================
        //Private Methods
        private JwtSecurityToken GenerateJWT(IEnumerable<Claim> claims)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));

            var signing = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            return new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims.ToArray(),
                expires: DateTime.Now.ToLocalTime().AddMinutes(_timeInMinutesValidJWT),
                signingCredentials: signing
             );
        }

        private IEnumerable<Claim> GenerateClaims(string name, IEnumerable<string> roles)
        {
            var claims = new List<Claim> {
                //Protect Before Replay attack
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name,name),
            };

            foreach (var item in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, item));
            }
            //new("Custom", "SomeData"),
            return claims;
        }
        //================================================================================================
    }
}
