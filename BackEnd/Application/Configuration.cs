using Application.Features.Addresses.Commands.Interfaces;
using Application.Features.Addresses.Commands.Services;
using Application.Features.Addresses.Mappers;
using Application.Features.Addresses.Queries.Interfaces;
using Application.Features.Addresses.Queries.Services;
using Application.Features.Characteristics.Mappers;
using Application.Features.Characteristics.Queries.Interfaces;
using Application.Features.Characteristics.Queries.Services;
using Application.Features.Companies.Commands.BranchOffers.Interfaces;
using Application.Features.Companies.Commands.BranchOffers.Services;
using Application.Features.Companies.Commands.CompanyBranches.Interfaces;
using Application.Features.Companies.Commands.CompanyBranches.Services;
using Application.Features.Companies.Mappers;
using Application.Features.Companies.Queries.PublicBranchOffer.Repositories;
using Application.Features.Companies.Queries.PublicBranchOffer.Services;
using Application.Features.Companies.Queries.PublicCompany.Interfaces;
using Application.Features.Companies.Queries.PublicCompany.Services;
using Application.Features.Companies.Queries.QueriesUser.Interfaces;
using Application.Features.Companies.Queries.QueriesUser.Services;
using Application.Features.Internships.Commands.Comments.Interfaces;
using Application.Features.Internships.Commands.Comments.Services;
using Application.Features.Internships.Commands.Internships.Interfaces;
using Application.Features.Internships.Commands.Internships.Services;
using Application.Features.Internships.Commands.Recrutments.Interfaces;
using Application.Features.Internships.Commands.Recrutments.Services;
using Application.Features.Internships.Mappers;
using Application.Features.Internships.Queries.Users.Interfaces;
using Application.Features.Internships.Queries.Users.Servises;
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
using Application.Shared.Services.Authentication;
using Application.Shared.Services.OrderBy;
using Domain.Features.Characteristic.Repositories;
using Domain.Features.Comment.Reposoitories;
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
            serviceCollection.AddTransient<IAuthJwtSvc, AuthJwtSvc>();
            serviceCollection.AddTransient<IOrderBySvc, OrderBySvc>();

            //Doamain Connections
            serviceCollection.AddTransient<IDomainNotificationDictionariesRepository, DomainNotificationDictionariesRepository>();
            serviceCollection.AddTransient<IDomainUrlTypeDictionariesRepository, DomainUrlTypeDictionariesRepository>();
            serviceCollection.AddTransient<ICommentTypeRepo, CommentTypeRepo>();

            //===============================================================================================================
            //User Module

            //Commands
            //User Part 
            serviceCollection.AddTransient<IUserMapper, UserMapper>();

            serviceCollection.AddTransient<IUserCommandRepository, UserCommandRepository>();
            serviceCollection.AddTransient<IUserCommandService, UserCommandService>();

            //Url Part 
            serviceCollection.AddTransient<IUrlCmdRepo, UrlCmdRepo>();
            serviceCollection.AddTransient<IUrlCmdSvc, UrlCmdSvc>();

            //Notification Part 
            serviceCollection.AddTransient<INotificationCmdSvc, NotificationCmdSvc>();
            serviceCollection.AddTransient<INotificationCmdRepo, NotificationCmdRepo>();

            //Queries
            serviceCollection.AddTransient<IUserQuerySvc, UserQuerySvc>();
            serviceCollection.AddTransient<IUserQueryRepo, UserQueryRepo>();

            //===============================================================================================================
            //Address Module
            serviceCollection.AddTransient<IAddressMapper, AddressMapper>();

            //Address Part
            serviceCollection.AddTransient<IAddressCmdRepo, AddressCmdRepo>();
            serviceCollection.AddTransient<IAddressCmdSvc, AddressCmdSvc>();

            serviceCollection.AddTransient<IAddressQueryRepo, AddressQueryRepo>();
            serviceCollection.AddTransient<IAddressQuerySvc, AddressQuerySvc>();

            //===============================================================================================================
            //Person Module 
            serviceCollection.AddTransient<IPersonMapper, PersonMapper>();

            //Person Part 
            serviceCollection.AddTransient<IPersonCmdRepo, PersonCmdRepo>();
            serviceCollection.AddTransient<IPersonCmdSvc, PersonCmdSvc>();

            //===============================================================================================================
            //Company Module 
            serviceCollection.AddTransient<ICompanyMapper, CompanyMapper>();

            //Company Branch Part 
            serviceCollection.AddTransient<ICompanyBranchCmdRepo, CompanyBranchCmdRepo>();
            serviceCollection.AddTransient<ICompanyBranchCmdSvc, CompanyBranchCmdSvc>();

            //Offer Branch Part 
            serviceCollection.AddTransient<IBranchOfferRepository, BranchOfferRepository>();
            serviceCollection.AddTransient<IBranchOfferCommandService, BranchOfferCommandService>();

            //Queries
            serviceCollection.AddTransient<IUserCompanyQueryRepo, UserCompanyQueryRepo>();
            serviceCollection.AddTransient<IUserCompanyQuerySvc, UserCompanyQuerySvc>();

            serviceCollection.AddTransient<IBranchOfferQueryRepo, BranchOfferQueryRepo>();
            serviceCollection.AddTransient<IBranchOfferQuerySvc, BranchOfferQuerySvc>();

            serviceCollection.AddTransient<ICompanyQueryRepo, CompanyQueryRepo>();
            serviceCollection.AddTransient<ICompanyQuerySvc, CompanyQuerySvc>();
            //===============================================================================================================
            //Intership module 
            serviceCollection.AddTransient<IInternshipMapper, InternshipMapper>();

            //Recruitment part
            serviceCollection.AddTransient<IRecruitmentCmdRepo, RecruitmentCmdRepo>();
            serviceCollection.AddTransient<IRecruitmentCmdSvc, RecruitmentCmdSvc>();
            //Internship Part
            serviceCollection.AddTransient<IInternshipCmdRepo, InternshipCmdRepo>();
            serviceCollection.AddTransient<IInternshipCmdSvc, InternshipCmdSvc>();
            //ComentPart
            serviceCollection.AddTransient<ICommentRepo, CommentRepo>();
            serviceCollection.AddTransient<ICommentCmdSvc, CommentSvc>();

            serviceCollection.AddTransient<IUsersInternshipsRepo, UsersInternshipsRepo>();
            serviceCollection.AddTransient<IUsersInternshipsQuerySvc, UsersInternshipsQuerySvc>();

            //===============================================================================================================
            //Characteristic Part
            serviceCollection.AddTransient<ICharacteristicMapper, CharacteristicMapper>();

            //Characteristic Part
            serviceCollection.AddTransient<ICharacteristicQueryRepository, CharacteristicQueryRepo>();
            serviceCollection.AddTransient<ICharacteristicQuerySvc, CharacteristicQuerySvc>();

            return serviceCollection;
        }
    }
}
