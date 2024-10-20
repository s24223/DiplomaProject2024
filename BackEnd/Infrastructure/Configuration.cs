using Application.Databases.Relational;
using Application.Features.Addresses.Interfaces;
using Infrastructure.Databases.Relational.MsSQL;
using Infrastructure.Databases.Relational.MsSQL.SqlClientRepositories;
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

            //DbContext Injection
            serviceCollection.AddTransient<DiplomaProjectContext, DiplomaProjectMsSQLContext>();

            return serviceCollection;
        }
    }
}
