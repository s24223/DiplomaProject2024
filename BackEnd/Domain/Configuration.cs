using Domain.Factories;
using Domain.Providers;
using Domain.Providers.ExceptionMessage;
using Domain.Providers.Time;
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
            serviceCollection.AddTransient<IDomainProvider, DomainProvider>();
            serviceCollection.AddTransient<IDomainFactory, DomainFactory>();


            serviceCollection.AddTransient<IExceptionMessageProvider, ExceptionMessageProvider>();
            serviceCollection.AddTransient<ITimeProvider, Providers.Time.TimeProvider>();

            return serviceCollection;
        }
    }
}
