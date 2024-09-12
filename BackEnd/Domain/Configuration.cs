using Microsoft.Extensions.DependencyInjection;

namespace Domain
{
    public static class Configuration
    {
        public static IServiceCollection DomainConfiguration(this IServiceCollection serviceCollection)
        {


            return serviceCollection;
        }
    }
}
