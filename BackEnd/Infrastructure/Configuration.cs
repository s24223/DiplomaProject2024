using Application.Databases.NonRelational;
using Application.Databases.Relational;
using Application.Shared.Interfaces.Email;
using Application.Shared.Interfaces.SqlClient;
using Infrastructure.Databases.NonRelational;
using Infrastructure.Databases.Relational.MsSQL;
using Infrastructure.Databases.Relational.MsSQL.SqlClientRepositories;
using Infrastructure.Email;
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
            serviceCollection.AddTransient<ISqlClientRepo, SqlClientRepo>();

            //DbContext Injection
            serviceCollection.AddTransient<DiplomaProjectContext, DiplomaProjectMsSQLContext>();
            //Email Service
            serviceCollection.AddTransient<IEmailService, EmailService>();
            serviceCollection.AddTransient<NonRelationalDbRepository, MongoDbRepository>();

            return serviceCollection;
        }
    }
}
