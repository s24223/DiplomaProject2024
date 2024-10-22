using Application.Features.User.DTOs.CommandsUser;
using Application.Shared.DTOs.Notifications;
using Domain.Features.User.Entities;

namespace Application.Features.User.DTOs.QueriesUser.FullUserData
{
    public class UserQueryResponseDto
    {
        //Values
        public Guid UserId { get; set; }
        public DateTime? LastLoginIn { get; set; } = null;
        public DateTime LastPasswordUpdate { get; set; }
        public IEnumerable<UrlResponseDto> Urls { get; set; } = new List<UrlResponseDto>();
        public IEnumerable<NotificationResponseDto> Notifications { get; set; } = new List<NotificationResponseDto>();


        public UserQueryResponseDto(DomainUser user)
        {
            UserId = user.Id.Value;
            LastLoginIn = user.LastLoginIn;
            LastPasswordUpdate = user.LastPasswordUpdate;

            Urls = user.Urls.Select(x => new UrlResponseDto(x.Value)).ToList();
            Notifications = user.Notifications.Select(x => new NotificationResponseDto(x.Value)).ToList();
        }
    }
}
