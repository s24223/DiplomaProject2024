using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class Configuration
    {
        public static IServiceCollection InfrastructureConfiguration(this IServiceCollection serviceCollection)
        {


            return serviceCollection;
        }
    }
}
