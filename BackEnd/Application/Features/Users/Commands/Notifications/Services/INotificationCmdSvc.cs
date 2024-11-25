using Application.Features.Users.Commands.Notifications.DTOs.Create;
using Application.Shared.DTOs.Features.Users.Notifications;
using Application.Shared.DTOs.Response;
using System.Security.Claims;

namespace Application.Features.Users.Commands.Notifications.Services
{
    public interface INotificationCmdSvc
    {
        //Unautorize
        Task<ResponseItem<NotificationResp>> CreateUnauthorizeAsync
            (
            CreateUnAuthNotificationReq dto,
            CancellationToken cancellation
            );


        //Autorize
        Task<ResponseItem<NotificationResp>> CreateAuthorizeAsync
            (
            IEnumerable<Claim> claims,
            CreateAuthNotificationReq dto,
            CancellationToken cancellation
            );

        Task<ResponseItem<NotificationResp>> AnnulAsync
            (
            IEnumerable<Claim> claims,
            Guid notificationId,
            CancellationToken cancellation
            );

        Task<ResponseItem<NotificationResp>> ReadAsync
            (
            IEnumerable<Claim> claims,
            Guid notificationId,
            CancellationToken cancellation
            );
    }
}
