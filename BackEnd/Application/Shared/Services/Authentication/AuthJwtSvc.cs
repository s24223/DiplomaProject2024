using Application.Shared.Exceptions.AppExceptions;
using Domain.Features.User.Exceptions.Entities;
using Domain.Features.User.ValueObjects.Identificators;
using Domain.Shared.Providers;
using Domain.Shared.Templates.Exceptions;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Application.Shared.Services.Authentication
{
    public class AuthJwtSvc : IAuthJwtSvc
    {
        //Values
        private readonly IProvider _provider;
        private readonly IConfiguration _configuration;

        private readonly string _issuer;
        private readonly string _audience;
        private readonly string _secret;

        private readonly DateTime _now;

        private readonly int _iterationCountOfHashPassword = 10000;
        private readonly int _timeInMinutesValidJWT = 100;
        private readonly int _timeInHourValidRefreshToken = 48;

        private readonly string _personRole = "person";
        private readonly string _companyRole = "company";


        //Constructor
        public AuthJwtSvc
            (
            IProvider provider,
            IConfiguration configuration
            )
        {
            _provider = provider;
            _configuration = configuration;

            var jwtSection = _configuration.GetSection("JwtData");

            var issuer = jwtSection["Issuer"];
            var audience = jwtSection["Audience"];
            var secret = jwtSection["Secret"];

            if (string.IsNullOrWhiteSpace(issuer))
            {
                throw new ApplicationLayerException(Messages.UserSecrets_Issuer_NotConfigured);
            }
            if (string.IsNullOrWhiteSpace(audience))
            {
                throw new ApplicationLayerException(Messages.UserSecrets_Audience_NotConfigured);
            }
            if (string.IsNullOrWhiteSpace(secret))
            {
                throw new ApplicationLayerException(Messages.UserSecrets_Secret_NotConfigured);
            }

            _issuer = issuer;
            _audience = audience;
            _secret = secret;
            _now = _provider.TimeProvider().GetDateTimeNow();
        }


        //=====================================================================================================
        //=====================================================================================================
        //=====================================================================================================
        //Public Methods
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
        public (string RefreshToken, DateTime ValidTo) GenerateRefreshTokendAndDateTimeValidTo()
        {
            var refresh = GenerateRefreshToken();
            var valid = _now.AddHours(_timeInHourValidRefreshToken);
            return (refresh, valid);
        }

        public (string Jwt, DateTime ValidTo) GenerateJwtStringAndDateTimeValidTo
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
        public bool IsJwtGeneratedByThisServer(string jwt)
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

        public bool IsJwtGeneratedByThisServerAndNotExpired(string jwt)
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
        public string GetPersonRole() => _personRole;
        public string GetCompanyRole() => _companyRole;

        public UserId GetIdNameFromJwt(string jwt)
        {
            if (!IsJwtGeneratedByThisServer(jwt))
            {
                throw new UserException
                    (
                    Messages.User_Jwt_IsNotGeneratedByThisServer,
                    DomainExceptionTypeEnum.Unauthorized
                    );
            }
            var claims = GetClaimsFromJwt(jwt);
            var id = GetIdNameFromClaims(claims);
            return id;
        }

        public UserId GetIdNameFromClaims(IEnumerable<Claim> claims)
        {
            var name = GetNameFromClaims(claims);
            if (!Guid.TryParse(name, out var id))
            {
                throw new UserException
                    (
                    $"{Messages.User_Jwt_NameIsNotGuid}: {name}",
                    DomainExceptionTypeEnum.AppProblem
                    );
            }
            return new UserId(id);
        }

        //================================================================================================
        //================================================================================================
        //================================================================================================
        //Private Methods


        //Private part Generators
        private string GenerateRefreshToken()
        {
            //Generate a 128-bit RefreshToken using secure PRNG
            byte[] salt = new byte[1024];
            using (var genNum = RandomNumberGenerator.Create())
            {
                genNum.GetBytes(salt);
            }
            return Convert.ToBase64String(salt);
        }

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


        //Private part Getters
        private IEnumerable<Claim> GetClaimsFromJwt(string jwt)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(jwt);
            var claims = jwtToken.Claims.ToList();
            return claims;
        }

        private string GetNameFromClaims(IEnumerable<Claim> claims)
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
    }
}
