using Domain.Shared.Templates.Exceptions;

namespace Domain.Features.Company.Exceptions.ValueObjects
{
    public class RegonException : DomainException
    {
        public RegonException
            (
            string? message,
            DomainExceptionTypeEnum type = DomainExceptionTypeEnum.BadInputData
            ) : base(message, type)
        {
        }
    }
}
