using Domain.Features.Notification.ValueObjects;

namespace Domain.Features.User.Repositories
{
    public interface IDomainUserDictionariesRepository
    {
        Dictionary<int, DomainNotificationSender> GetNotificationSenders();
        Dictionary<int, DomainNotificationStatus> GetNotificationStatuses();
    }
}
