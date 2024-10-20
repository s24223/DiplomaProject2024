using Application.Databases.Relational;
using Application.Shared.Services.Authentication;
using Domain.Shared.Providers;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Middlewares.CustomMiddlewares
{
    public class AuthenticationWerifierMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthenticationWerifierMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke
            (
            HttpContext context,
            DiplomaProjectContext dBContext,
            IAuthenticationService authenticationRepository,
            IProvider domainRepository,
            IConfiguration configuration
            )
        {
            if (context.Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
            {
                var jwt = authorizationHeader.ToString().Replace("Bearer ", "");


                var isValidJWT = authenticationRepository.IsJwtGeneratedByThisServer(jwt);

                if (!isValidJWT)
                {
                    context.Response.StatusCode = 401;
                    return;
                }

                var id = authenticationRepository.GetIdNameFromJwt(jwt);

                var user = await dBContext.Users.Where(x => x.Id == id.Value).AsNoTracking().FirstOrDefaultAsync();
                if (
                    user == null ||
                    string.IsNullOrWhiteSpace(user.RefreshToken) ||
                    user.ExpiredToken == null ||
                    user.ExpiredToken <= domainRepository.TimeProvider().GetDateTimeNow()
                    )
                {
                    context.Request.Headers.Remove("Authorization");
                }
            }
            await _next(context);
        }
    }
}
