using Application.Features.Addresses.Commands.Interfaces;
using Application.Features.Addresses.Commands.Services;
using Application.Features.Addresses.Queries.Interfaces;
using Application.Features.Addresses.Queries.Services;
using Application.Features.Characteristics.Mappers;
using Application.Features.Characteristics.Queries.Interfaces;
using Application.Features.Characteristics.Queries.Services;
using Application.Features.Companies.Interfaces.CommandsBranchOffer;
using Application.Features.Companies.Interfaces.CommandsCompanyBranch;
using Application.Features.Companies.Interfaces.QueriesOffer;
using Application.Features.Companies.Mappers.DatabaseToDomain;
using Application.Features.Companies.Services.CommandsBranchOffer;
using Application.Features.Companies.Services.CommandsCompanyBranch;
using Application.Features.Companies.Services.QueriesOffer;
using Application.Features.Internship.InternshipPart.Interfaces;
using Application.Features.Internship.InternshipPart.Services;
using Application.Features.Internship.RecrutmentPart.Interfaces;
using Application.Features.Internship.RecrutmentPart.Services;
using Application.Features.Persons.Commands.Interfaces;
using Application.Features.Persons.Commands.Services;
using Application.Features.Persons.Mappers;
using Application.Features.Users.Commands.Notifications.Interfaces;
using Application.Features.Users.Commands.Notifications.Services;
using Application.Features.Users.Commands.Urls.Interfaces;
using Application.Features.Users.Commands.Urls.Services;
using Application.Features.Users.Commands.Users.Interfaces;
using Application.Features.Users.Commands.Users.Services;
using Application.Features.Users.Mappers;
using Application.Features.Users.Queries.QueriesUser.Interfaces;
using Application.Features.Users.Queries.QueriesUser.Services;
using Application.Shared.Interfaces.DomainRepositories;
using Application.Shared.Interfaces.EntityToDomainMappers;
using Application.Shared.Interfaces.Exceptions;
using Application.Shared.Services.Authentication;
using Domain.Features.Characteristic.Repositories;
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
            serviceCollection.AddTransient<IUserMapper, UserMapper>();

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
            serviceCollection.AddTransient<IPersonCmdRepository, PersonCmdRepository>();
            serviceCollection.AddTransient<IPersonCmdService, PersonCmdService>();
            serviceCollection.AddTransient<IPersonMapper, PersonMapper>();

            //===============================================================================================================
            //Company Module 
            serviceCollection.AddTransient<ICompanyMapper, CompanyMapper>();

            //Company Branch Part 
            serviceCollection.AddTransient<ICompanyBranchCommandRepository, CompanyBranchCommandRepository>();
            serviceCollection.AddTransient<ICompanyBranchCommandService, CompanyBranchCommandService>();

            //Offer Branch Part 
            serviceCollection.AddTransient<IBranchOfferRepository, BranchOfferRepository>();
            serviceCollection.AddTransient<IBranchOfferCommandService, BranchOfferCommandService>();

            //Queries
            serviceCollection.AddTransient<IOfferQueryRepository, OfferQueryRepository>();
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
            //Characteristic Part
            serviceCollection.AddTransient<ICharacteristicQueryRepository, CharacteristicQueryRepository>();
            serviceCollection.AddTransient<ICharacteristicQueryService, CharacteristicQueryService>();

            serviceCollection.AddTransient<ICharacteristicMapper, CharacteristicMapper>();

            return serviceCollection;
        }
    }
}
