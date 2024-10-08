using Domain.Shared.Templates.Exceptions;

namespace Domain.Features.Url.Exceptions.ValueObjects
{
    public class UrlTypeException : DomainException
    {
        public UrlTypeException
            (
            string? message,
            DomainExceptionTypeEnum type = DomainExceptionTypeEnum.BadInputData
            ) : base(message, type)
        {
        }
    }
}
