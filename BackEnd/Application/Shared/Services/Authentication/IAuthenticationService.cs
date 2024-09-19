using System.Security.Claims;

namespace Application.Shared.Services.Authentication
{
    public interface IAuthenticationService
    {
        //Password Part
        string GenerateSalt();
        string HashPassword(string password, string salt);

        //Jwt Part
        //Generators
        (string RefreshToken, DateTime ValidTo) GenerateRefreshTokendAndDateTimeValidTo();
        (string Jwt, DateTime ValidTo) GenerateJwtStringAndDateTimeValidTo
            (
            string name,
            IEnumerable<string> roles
            );

        //Validation
        bool IsJwtGeneratedByThisServer(string jwt);
        bool IsJwtGeneratedByThisServerAndNotExpired(string jwt);

        //Getters
        string GetPersonRole();
        string GetCompanyRole();
        Guid GetIdNameFromJwt(string jwt);
        Guid GetIdNameFromClaims(IEnumerable<Claim> claims);
    }
}
