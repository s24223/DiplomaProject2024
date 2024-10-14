using Domain.Shared.Templates.Exceptions;

namespace Domain.Shared.Exceptions.ValueObjects
{
    public class EmailException : DomainException
    {
        public EmailException
            (
            string? message,
            DomainExceptionTypeEnum type = DomainExceptionTypeEnum.BadInputData
            ) : base(message, type)
        {
        }
    }
}
