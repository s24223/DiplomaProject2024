using Application.Shared.DTOs.Features.Users.Notifications;
using Domain.Features.Notification.Entities;

namespace Application.Features.Users.Queries.QueriesUser.DTOs
{
    public class GetNotificationsResp
    {
        //Values
        public IEnumerable<NotificationResp> Urls { get; private set; } = [];
        public int Count { get; private set; }
        public int TotalCount { get; private set; }


        //Constructor
        public GetNotificationsResp(int totalCount, IEnumerable<DomainNotification> urls)
        {
            TotalCount = totalCount;
            Urls = urls.Select(x => new NotificationResp(x));
            Count = urls.Count();
        }
    }
}
