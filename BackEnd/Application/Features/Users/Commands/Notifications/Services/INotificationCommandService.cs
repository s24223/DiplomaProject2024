using Application.Features.Users.Commands.Notifications.DTOs.Create;
using Application.Features.Users.Commands.Notifications.DTOs.Create.Authorize;
using Application.Features.Users.Commands.Notifications.DTOs.Create.Unauthorize;
using Application.Shared.DTOs.Response;
using System.Security.Claims;

namespace Application.Features.Users.Commands.Notifications.Services
{
    public interface INotificationCommandService
    {
        //Unautorize
        Task<ResponseItem<CreateNotificationRequestDto>> CreateUnauthorizeAsync
            (
            CreateUnauthorizeNotificationRequestDto dto,
            CancellationToken cancellation
            );


        //Autorize
        Task<ResponseItem<CreateNotificationRequestDto>> CreateAuthorizeAsync
            (
            IEnumerable<Claim> claims,
            CreateAuthorizeNotificationRequestDto dto,
            CancellationToken cancellation
            );

        Task<Response> AnnulAsync
            (
            IEnumerable<Claim> claims,
            Guid notificationId,
            CancellationToken cancellation
            );

        Task<Response> ReadAsync
            (
            IEnumerable<Claim> claims,
            Guid notificationId,
            CancellationToken cancellation
            );
    }
}
