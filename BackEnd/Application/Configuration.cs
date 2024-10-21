using Application.Features.Addresses.Interfaces;
using Application.Features.Addresses.Services;
using Application.Features.Companies.Interfaces.CommandsBranchOffer;
using Application.Features.Companies.Interfaces.CommandsCompanyBranch;
using Application.Features.Companies.Services.CommandsBranchOffer;
using Application.Features.Companies.Services.CommandsCompanyBranch;
using Application.Features.Internship.InternshipPart.Interfaces;
using Application.Features.Internship.InternshipPart.Services;
using Application.Features.Internship.RecrutmentPart.Interfaces;
using Application.Features.Internship.RecrutmentPart.Services;
using Application.Features.Person.Interfaces;
using Application.Features.Person.Services;
using Application.Features.User.Interfaces.CommandsUrl;
using Application.Features.User.Interfaces.CommandsUser;
using Application.Features.User.Services.CommandsUrl;
using Application.Features.User.Services.CommandsUser;
using Application.Shared.Interfaces.EntityToDomainMappers;
using Application.Shared.Interfaces.Exceptions;
using Application.Shared.Repositories;
using Application.Shared.Services.Authentication;
using Domain.Features.Notification.Repositories;
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
            serviceCollection.AddTransient<IUserCommandRepository, UserCommandRepository>();
            serviceCollection.AddTransient<IUserCommandService, UserCommandService>();

            //Url Part
            serviceCollection.AddTransient<IUrlCommandRepository, UrlCommandRepository>();
            serviceCollection.AddTransient<IUrlCommandService, UrlCommandService>();

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


            //Doamin Connections
            serviceCollection.AddTransient<IDomainUserDictionariesRepository, DomainUserDictionariesRepository>();


            return serviceCollection;
        }
    }
}
