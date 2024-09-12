using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class Configuration
    {
        public static IServiceCollection ApplicationConfiguration(this IServiceCollection serviceCollection)
        {


            return serviceCollection;
        }
    }
}
