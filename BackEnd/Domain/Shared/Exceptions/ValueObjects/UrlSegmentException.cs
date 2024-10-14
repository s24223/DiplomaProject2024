using Domain.Shared.Templates.Exceptions;

namespace Domain.Shared.Exceptions.ValueObjects
{
    public class UrlSegmentException : DomainException
    {
        public UrlSegmentException
            (
            string? message,
            DomainExceptionTypeEnum type = DomainExceptionTypeEnum.BadInputData
            ) : base(message, type)
        {
        }
    }
}
