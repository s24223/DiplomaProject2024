using Application.Features.Addresses.Interfaces;
using Application.Features.Addresses.Services;
using Application.Features.Companies.BranchOfferPart.Interfaces;
using Application.Features.Companies.BranchOfferPart.Services;
using Application.Features.Companies.BranchPart.Interfaces;
using Application.Features.Companies.BranchPart.Services;
using Application.Features.Companies.CompanyPart.Interfaces;
using Application.Features.Companies.CompanyPart.Services;
using Application.Features.Internship.InternshipPart.Interfaces;
using Application.Features.Internship.InternshipPart.Services;
using Application.Features.Internship.RecrutmentPart.Interfaces;
using Application.Features.Internship.RecrutmentPart.Services;
using Application.Features.Person.Interfaces;
using Application.Features.Person.Services;
using Application.Features.User.UrlPart.Interfaces;
using Application.Features.User.UrlPart.Services;
using Application.Features.User.UserPart.Interfaces;
using Application.Features.User.UserPart.Services;
using Application.Features.User.UserProblemPart.Interfaces;
using Application.Features.User.UserProblemPart.Services;
using Application.Shared.Interfaces.EntityToDomainMappers;
using Application.Shared.Interfaces.Exceptions;
using Application.Shared.Services.Authentication;
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
            serviceCollection.AddTransient<IEntityToDomainMapper, EntityToDomainMapper>();


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

            //Company Part 
            serviceCollection.AddTransient<ICompanyRepository, CompanyRepository>();
            serviceCollection.AddTransient<ICompanyService, CompanyService>();

            //Offer Part 
            serviceCollection.AddTransient<IBranchOfferRepository, BranchOfferRepository>();
            serviceCollection.AddTransient<IBranchOfferService, BranchOfferService>();

            //Branch Part
            serviceCollection.AddTransient<IBranchRepository, BranchRepository>();
            serviceCollection.AddTransient<IBranchService, BranchService>();

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
