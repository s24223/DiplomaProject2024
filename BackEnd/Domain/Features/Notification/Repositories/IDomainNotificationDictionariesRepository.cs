using Domain.Features.Notification.ValueObjects;

namespace Domain.Features.Notification.Repositories
{
    public interface IDomainNotificationDictionariesRepository
    {
        Dictionary<int, DomainNotificationSender> GetNotificationSenders();
        Dictionary<int, DomainNotificationStatus> GetNotificationStatuses();
    }
}
