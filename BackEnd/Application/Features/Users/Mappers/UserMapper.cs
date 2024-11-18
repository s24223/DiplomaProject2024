using Application.Databases.Relational.Models;
using Domain.Features.Notification.Entities;
using Domain.Features.Url.Entities;
using Domain.Features.User.Entities;
using Domain.Shared.Factories;

namespace Application.Features.Users.Mappers
{
    public class UserMapper : IUserMapper
    {
        //Values
        private readonly IDomainFactory _domainFactory;


        //Cosnructor
        public UserMapper
            (
            IDomainFactory domainFactory
            )
        {
            _domainFactory = domainFactory;
        }


        //================================================================================================================
        //================================================================================================================
        //================================================================================================================
        //Public Methods
        public DomainUser DomainUser(User databaseUser)
        {
            return _domainFactory.CreateDomainUser
                (
                databaseUser.Id,
                databaseUser.Login,
                databaseUser.LastLoginIn,
                databaseUser.LastPasswordUpdate
                );
        }


        public DomainUrl DomainUrl(Url databaseUrl)
        {
            return _domainFactory.CreateDomainUrl
                (
                databaseUrl.UserId,
                databaseUrl.UrlTypeId,
                databaseUrl.Created,
                databaseUrl.Path,
                databaseUrl.Name,
                databaseUrl.Description
                );
        }

        public DomainNotification DomainNotification(Notification notification)
        {
            return _domainFactory.CreateDomainNotification
                (
                notification.Id,
                notification.UserId,
                notification.Email,
                notification.Created,
                notification.Completed,
                notification.PreviousProblemId,
                notification.IdAppProblem,
                notification.UserMessage,
                notification.Response,
                notification.IsReadedByUser,
                notification.NotificationSenderId,
                notification.NotificationStatusId
                );
        }
        //================================================================================================================
        //================================================================================================================
        //================================================================================================================
        //Private Methods
    }
}
