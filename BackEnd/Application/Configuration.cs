using Application.Shared.Interfaces.Exceptions;
using Application.Shared.Services.Authentication;
using Application.VerticalSlice.AddressPart.Interfaces;
using Application.VerticalSlice.AddressPart.Services;
using Application.VerticalSlice.CompanyPart.Interfaces;
using Application.VerticalSlice.CompanyPart.Services;
using Application.VerticalSlice.InternshipPart.Interfaces;
using Application.VerticalSlice.InternshipPart.Services;
using Application.VerticalSlice.OfferBranchPart.Interfaces;
using Application.VerticalSlice.OfferBranchPart.Services;
using Application.VerticalSlice.OfferPart.Interfaces;
using Application.VerticalSlice.OfferPart.Services;
using Application.VerticalSlice.PersonPart.Interfaces;
using Application.VerticalSlice.PersonPart.Services;
using Application.VerticalSlice.RecrutmentPart.Interfaces;
using Application.VerticalSlice.RecrutmentPart.Services;
using Application.VerticalSlice.UrlPart.Interfaces;
using Application.VerticalSlice.UrlPart.Services;
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
            serviceCollection.AddTransient<IExceptionsRepository, ExceptionsRepository>();


            //User Part
            serviceCollection.AddTransient<IUserRepository, UserRepository>();
            serviceCollection.AddTransient<IUserService, UserService>();

            //UserProblem Part
            serviceCollection.AddTransient<IUserProblemRepository, UserProblemRepository>();
            serviceCollection.AddTransient<IUserProblemService, UserProblemService>();

            //Url Part
            serviceCollection.AddTransient<IUrlRepository, UrlRepository>();
            serviceCollection.AddTransient<IUrlService, UrlService>();

            //Address Part
            serviceCollection.AddTransient<IAddressRepository, AddressRepository>();
            serviceCollection.AddTransient<IAddressService, AddressService>();

            //Person Part 
            serviceCollection.AddTransient<IPersonRepository, PersonRepository>();
            serviceCollection.AddTransient<IPersonService, PersonService>();

            //xxx
            //Company Part 
            serviceCollection.AddTransient<ICompanyRepository, CompanyRepository>();
            serviceCollection.AddTransient<ICompanyService, CompanyService>();

            //Offer Part 
            serviceCollection.AddTransient<IOfferRepository, OfferRepository>();
            serviceCollection.AddTransient<IOfferService, OfferService>();

            //OfferBranch Part
            serviceCollection.AddTransient<IBranchOfferRepository, OfferBranchRepository>();
            serviceCollection.AddTransient<IBranchOfferService, BranchOfferService>();

            //Recruitment
            serviceCollection.AddTransient<IRecruitmentRepository, RecruitmentRepository>();
            serviceCollection.AddTransient<IRecruitmentService, RecruitmentService>();

            //Internship Part
            serviceCollection.AddTransient<IInternshipRepository, InternshipRepository>();
            serviceCollection.AddTransient<IInternshipService, InternshipService>();


            return serviceCollection;
        }
    }
}
