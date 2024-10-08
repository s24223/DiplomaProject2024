using Domain.Shared.Templates.Exceptions;

namespace Domain.Features.Url.Exceptions.Entities
{
    public class UrlException : DomainException
    {
        public UrlException
            (
            string? message,
            DomainExceptionTypeEnum type = DomainExceptionTypeEnum.BadInputData
            ) : base(message, type)
        {
        }
    }
}
