using System.Security.Claims;

namespace Application.Shared.Repositories.Authentication
{
    public interface IAuthenticationRepository
    {
        //Password Part
        string GenerateSalt();
        string HashPassword(string password, string salt);

        //Jwt Part
        //Generators
        string GenerateRefreshToken();
        (string, DateTime) GenerateJWTStringAndDateTimeValidTo
            (
            string name,
            IEnumerable<string> roles
            );
        //Validation
        bool IsJWTGeneratedByThisServer(string jwt);
        bool IsJWTGeneratedByThisServerAndNotExpired(string jwt);
        //Getters
        IEnumerable<Claim> GetClaimsFromJWT(string jwt);
        string GetNameFromClaims(IEnumerable<Claim> claims);
        int GetTimeInHourValidRefreshToken();
    }
}
