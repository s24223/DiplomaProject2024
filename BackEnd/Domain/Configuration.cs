using Domain.Features.Characteristic.Factories;
using Domain.Shared.Factories;
using Domain.Shared.Providers;
using Domain.Shared.Providers.ExceptionMessage;
using Domain.Shared.Providers.Time;
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
            serviceCollection.AddTransient<IDomainFactory, DomainFactory>();

            //Provider
            serviceCollection.AddTransient<IProvider, Provider>();

            serviceCollection.AddTransient<IExceptionMessageProvider, ExceptionMessageProvider>();
            serviceCollection.AddTransient<ITimeProvider, Shared.Providers.Time.TimeProvider>();

            serviceCollection.AddTransient<ICharacteristicFactory, CharacteristicFactory>();

            return serviceCollection;
        }
    }
}
