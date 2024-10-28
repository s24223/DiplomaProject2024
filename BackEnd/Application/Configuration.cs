using Application.Features.Addresses.Interfaces.Commands;
using Application.Features.Addresses.Interfaces.Queries;
using Application.Features.Addresses.Services.Commands;
using Application.Features.Addresses.Services.Queries;
using Application.Features.Companies.Interfaces.CommandsBranchOffer;
using Application.Features.Companies.Interfaces.CommandsCompanyBranch;
using Application.Features.Companies.Interfaces.QueriesCompany;
using Application.Features.Companies.Services.CommandsBranchOffer;
using Application.Features.Companies.Services.CommandsCompanyBranch;
using Application.Features.Companies.Services.QueriesCompany;
using Application.Features.Internship.InternshipPart.Interfaces;
using Application.Features.Internship.InternshipPart.Services;
using Application.Features.Internship.RecrutmentPart.Interfaces;
using Application.Features.Internship.RecrutmentPart.Services;
using Application.Features.Person.Interfaces;
using Application.Features.Person.Services;
using Application.Features.User.Interfaces.CommandsNotification;
using Application.Features.User.Interfaces.CommandsUrl;
using Application.Features.User.Interfaces.CommandsUser;
using Application.Features.User.Interfaces.QueriesUser;
using Application.Features.User.Services.CommandsNotification;
using Application.Features.User.Services.CommandsUrl;
using Application.Features.User.Services.CommandsUser;
using Application.Features.User.Services.QueriesUser;
using Application.Shared.Interfaces.EntityToDomainMappers;
using Application.Shared.Interfaces.Exceptions;
using Application.Shared.Repositories;
using Application.Shared.Services.Authentication;
using Domain.Features.Notification.Repositories;
using Domain.Features.Url.Repository;
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

            //Doamain Connections
            serviceCollection.AddTransient<IDomainNotificationDictionariesRepository, DomainNotificationDictionariesRepository>();
            serviceCollection.AddTransient<IDomainUrlTypeDictionariesRepository, DomainUrlTypeDictionariesRepository>();

            //===============================================================================================================
            //User Module

            //Commands
            //User Part 
            serviceCollection.AddTransient<IUserCommandRepository, UserCommandRepository>();
            serviceCollection.AddTransient<IUserCommandService, UserCommandService>();

            //Url Part 
            serviceCollection.AddTransient<IUrlCommandRepository, UrlCommandRepository>();
            serviceCollection.AddTransient<IUrlCommandService, UrlCommandService>();

            //Notification Part 
            serviceCollection.AddTransient<INotificationCommandService, NotificationCommandService>();
            serviceCollection.AddTransient<INotificationCommandRepository, NotificationCommandRepository>();

            //Queries
            serviceCollection.AddTransient<IUserQueryService, UserQueryService>();
            serviceCollection.AddTransient<IUserQueryRepository, UserQueryRepository>();

            //===============================================================================================================
            //Address Module
            //Address Part
            serviceCollection.AddTransient<IAddressCommandRepository, AddressCommandRepository>();
            serviceCollection.AddTransient<IAddressCommandService, AddressCommandService>();

            serviceCollection.AddTransient<IAddressQueryRepository, AddressQueryRepository>();
            serviceCollection.AddTransient<IAddressQueryService, AddressQueryService>();

            //===============================================================================================================
            //Person Module 
            //Person Part 
            serviceCollection.AddTransient<IPersonRepository, PersonRepository>();
            serviceCollection.AddTransient<IPersonService, PersonService>();

            //===============================================================================================================
            //Company Module 
            //Company Branch Part 
            serviceCollection.AddTransient<ICompanyBranchCommandRepository, CompanyBranchCommandRepository>();
            serviceCollection.AddTransient<ICompanyBranchCommandService, CompanyBranchCommandService>();

            //Offer Branch Part 
            serviceCollection.AddTransient<IBranchOfferRepository, BranchOfferRepository>();
            serviceCollection.AddTransient<IBranchOfferService, BranchOfferService>();

            //Queries
            serviceCollection.AddTransient<ICompanyQueryRepository, CompanyQueryRepository>();
            serviceCollection.AddTransient<ICompanyQueryService, CompanyQueryService>();

            //===============================================================================================================
            //Intership module 
            //Recruitment part
            serviceCollection.AddTransient<IRecruitmentRepository, RecruitmentRepository>();
            serviceCollection.AddTransient<IRecruitmentService, RecruitmentService>();

            //Internship Part
            serviceCollection.AddTransient<IInternshipRepository, InternshipRepository>();
            serviceCollection.AddTransient<IInternshipService, InternshipService>();

            //===============================================================================================================
            return serviceCollection;
        }
    }
}
