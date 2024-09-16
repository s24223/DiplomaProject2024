using Domain.Factories;
using Domain.Repositories;
using Domain.Repositories.ExceptionMessage;
using Domain.Repositories.Time;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Domain
{
    public static class Configuration
    {
        public static IServiceCollection DomainConfiguration
            (
            this IServiceCollection serviceCollection,
            IConfiguration configuration
            )
        {
            // Rejestracja IConfiguration jako Singleton
            serviceCollection.AddSingleton<IConfiguration>(configuration);
            serviceCollection.AddTransient<IDomainRepository, DomainRepository>();
            serviceCollection.AddTransient<IDomainFactory, DomainFactory>();


            serviceCollection.AddTransient<IExceptionMessageRepository, ExceptionMessageRepository>();
            serviceCollection.AddTransient<ITimeRepository, TimeRepository>();

            return serviceCollection;
        }
    }
}
