using Application.Database;
using Infrastructure.MsSqlDatabase;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class Configuration
    {
        public static IServiceCollection InfrastructureConfiguration
            (
            this IServiceCollection serviceCollection,
            IConfiguration configuration
            )
        {
            // Rejestracja IConfiguration jako Singleton
            serviceCollection.AddSingleton<IConfiguration>(configuration);

            serviceCollection.AddTransient<DiplomaProjectContext, DiplomaProjectMsSqlContext>();
            return serviceCollection;
        }
    }
}
