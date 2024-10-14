using Domain.Shared.Templates.Exceptions;

namespace Domain.Features.Person.Exceptions.ValueObjects
{
    public class PhoneNumberException : DomainException
    {
        public PhoneNumberException
            (
            string? message,
            DomainExceptionTypeEnum type = DomainExceptionTypeEnum.BadInputData
            ) : base(message, type)
        {
        }
    }
}
