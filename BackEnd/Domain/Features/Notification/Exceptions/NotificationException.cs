using Domain.Shared.Templates.Exceptions;

namespace Domain.Features.Notification.Exceptions
{
    public class NotificationException : DomainException
    {
        public NotificationException
            (
            string? message,
            DomainExceptionTypeEnum type = DomainExceptionTypeEnum.BadInputData
            ) : base(message, type)
        {
        }
    }
}
