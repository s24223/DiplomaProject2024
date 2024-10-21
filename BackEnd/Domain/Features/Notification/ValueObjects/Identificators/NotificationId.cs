using Domain.Shared.Templates.ValueObjects.Identificators;

namespace Domain.Features.Notification.ValueObjects.Identificators
{
    public record NotificationId : GuidId
    {
        public NotificationId(Guid? value) : base(value)
        {
        }
    }
}
