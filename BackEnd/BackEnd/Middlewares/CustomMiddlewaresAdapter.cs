using BackEnd.Middlewares.CustomMiddlewares;

namespace BackEnd.Middlewares
{
    public static class CustomMiddlewaresAdapter
    {
        public static IApplicationBuilder UseAuthenticationWerifier(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthenticationWerifierMiddleware>();
        }
    }
}
