using Application.Features.Users.Commands.Notifications.DTOs.Create;
using Application.Features.Users.Commands.Notifications.Interfaces;
using Application.Shared.DTOs.Features.Users.Notifications;
using Application.Shared.DTOs.Response;
using Application.Shared.Services.Authentication;
using Domain.Features.Notification.Entities;
using Domain.Features.Notification.ValueObjects.Identificators;
using Domain.Features.User.ValueObjects.Identificators;
using Domain.Shared.Factories;
using System.Security.Claims;

namespace Application.Features.Users.Commands.Notifications.Services
{
    public class NotificationCmdSvc : INotificationCmdSvc
    {
        //Values
        private readonly IDomainFactory _domainFactory;
        private readonly IAuthJwtSvc _authentication;
        private readonly INotificationCmdRepo _notificationRepo;


        //Cosntructor
        public NotificationCmdSvc
            (
            IDomainFactory domainFactory,
            IAuthJwtSvc authentication,
            INotificationCmdRepo notificationRepository
            )
        {
            _domainFactory = domainFactory;
            _authentication = authentication;
            _notificationRepo = notificationRepository;
        }


        //===================================================================================================
        //===================================================================================================
        //===================================================================================================
        //Public Methods
        //Unautorize
        public async Task<ResponseItem<NotificationResp>> CreateUnauthorizeAsync
            (
            CreateUnAuthNotificationReq dto,
            CancellationToken cancellation
            )
        {
            var domainNotification = _domainFactory.CreateDomainNotification
                (
                null,
                dto.Email,
                dto.IdAppProblem,
                dto.PreviousProblemId,
                dto.UserMessage,
                1,
                1
                );

            domainNotification = await _notificationRepo.CreateAsync(domainNotification, cancellation);

            return new ResponseItem<NotificationResp>
            {
                Item = new NotificationResp(domainNotification),
            };
        }


        //Autorize
        public async Task<ResponseItem<NotificationResp>> CreateAuthorizeAsync
            (
            IEnumerable<Claim> claims,
            CreateAuthNotificationReq dto,
            CancellationToken cancellation
            )
        {
            var userId = GetUserId(claims);
            var domainNotification = _domainFactory.CreateDomainNotification
                (
                userId.Value,
                null,
                dto.PreviousProblemId,
                dto.IdAppProblem,
                dto.UserMessage,
                1,
                1
                );
            domainNotification = await _notificationRepo.CreateAsync(domainNotification, cancellation);

            return new ResponseItem<NotificationResp>
            {
                Item = new NotificationResp(domainNotification),
            };
        }

        public async Task<ResponseItem<NotificationResp>> AnnulAsync
            (
            IEnumerable<Claim> claims,
            Guid notificationId,
            CancellationToken cancellation
            )
        {
            return await HandleNotificationAsync
                (
                claims,
                notificationId,
                cancellation,
                domainNotification => domainNotification.Annul()
                );
        }

        public async Task<ResponseItem<NotificationResp>> ReadAsync
            (
            IEnumerable<Claim> claims,
            Guid notificationId,
            CancellationToken cancellation
            )
        {
            return await HandleNotificationAsync
                (
                claims,
                notificationId,
                cancellation,
                domainNotification => domainNotification.SetReadedByUser()
                );
        }

        //===================================================================================================
        //===================================================================================================
        //===================================================================================================
        //Private Methods
        private UserId GetUserId(IEnumerable<Claim> claims)
        {
            return _authentication.GetIdNameFromClaims(claims);
        }

        private async Task<ResponseItem<NotificationResp>> HandleNotificationAsync
            (
            IEnumerable<Claim> claims,
            Guid notificationId,
            CancellationToken cancellation,
            Action<DomainNotification> action
            )
        {
            var userId = GetUserId(claims);

            var domainNotification = await _notificationRepo
                .GetNotificationAsync(userId, new NotificationId(notificationId), cancellation);

            action(domainNotification);

            domainNotification = await _notificationRepo.UpdateAsync(userId, domainNotification, cancellation);

            return new ResponseItem<NotificationResp>
            {
                Item = new NotificationResp(domainNotification),
            };
        }
    }
}
