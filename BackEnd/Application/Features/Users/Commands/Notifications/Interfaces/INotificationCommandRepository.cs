using Domain.Features.Notification.Entities;
using Domain.Features.Notification.ValueObjects.Identificators;
using Domain.Features.User.ValueObjects.Identificators;

namespace Application.Features.Users.Commands.Notifications.Interfaces
{
    public interface INotificationCommandRepository
    {
        Task<Guid> CreateAsync
            (
            DomainNotification domainNotification,
            CancellationToken cancellation
            );

        Task UpdateAsync
            (
            UserId userId,
            DomainNotification domainNotification,
            CancellationToken cancellation
            );

        Task<DomainNotification> GetNotificationAsync
            (
            UserId userId,
            NotificationId id,
            CancellationToken cancellation
            );
    }
}
