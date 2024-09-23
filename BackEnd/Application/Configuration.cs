using Application.Shared.Services.Authentication;
using Application.VerticalSlice.AddressPart.Services;
using Application.VerticalSlice.CompanyPart.Interfaces;
using Application.VerticalSlice.CompanyPart.Services;
using Application.VerticalSlice.PersonPart.Interfaces;
using Application.VerticalSlice.PersonPart.Services;
using Application.VerticalSlice.UserPart.Interfaces;
using Application.VerticalSlice.UserPart.Services;
using Application.VerticalSlice.UserProblemPart.Interfaces;
using Application.VerticalSlice.UserProblemPart.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class Configuration
    {
        public static IServiceCollection ApplicationConfiguration
            (
            this IServiceCollection serviceCollection,
            IConfiguration configuration
            )
        {
            // Rejestracja IConfiguration jako Singleton
            serviceCollection.AddSingleton<IConfiguration>(configuration);
            serviceCollection.AddTransient<IAuthenticationService, AuthenticationService>();

            //User Part
            serviceCollection.AddTransient<IUserRepository, UserRepository>();
            serviceCollection.AddTransient<IUserService, UserService>();

            //UserProblem Part
            serviceCollection.AddTransient<IUserProblemRepository, UserProblemRepository>();
            serviceCollection.AddTransient<IUserProblemService, UserProblemService>();

            //Company Part 
            serviceCollection.AddTransient<ICompanyRepository, CompanyRepository>();
            serviceCollection.AddTransient<ICompanyService, CompanyService>();

            //Person Part 
            serviceCollection.AddTransient<IPersonRepository, PersonRepository>();
            serviceCollection.AddTransient<IPersonService, PersonService>();

            //Address Part
            serviceCollection.AddTransient<IAddressService, AddressService>();

            return serviceCollection;
        }
    }
}
