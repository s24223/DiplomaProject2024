using Application.Database;
using Application.VerticalSlice.AddressPart.Interfaces;
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

            //Address Part from Application
            serviceCollection.AddTransient<IAddressSqlClientRepository, AddressSqlClientRepository>();


            serviceCollection.AddTransient<DiplomaProjectContext, DiplomaProjectMsSqlContext>();
            return serviceCollection;
        }
    }
}
