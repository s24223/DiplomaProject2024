using Domain.Features.Notification.Repositories;
using Domain.Features.Notification.ValueObjects;

namespace DomainTests.Fakes
{
    public class DomainNotificationsFake : IDomainNotificationDictionariesRepository
    {
        public Dictionary<int, DomainNotificationSender> GetNotificationSenders()
        {
            return new Dictionary<int, DomainNotificationSender>
            {
                {1 , new DomainNotificationSender(1 ,"User", "User Description") },
            };
        }

        public Dictionary<int, DomainNotificationStatus> GetNotificationStatuses()
        {
            return new Dictionary<int, DomainNotificationStatus>
            {
                {1 , new DomainNotificationStatus(1, "Created") },
                {2 , new DomainNotificationStatus(2, "Answered") },
                {3 , new DomainNotificationStatus(3, "Annul") },
                {4 , new DomainNotificationStatus(4, "Done") },
            };
        }
    }
}
