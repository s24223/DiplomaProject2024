using Domain.Shared.Templates.Exceptions;

namespace Domain.Features.Address.Exceptions.ValueObjects
{
    public class ZipCodeException : DomainException
    {
        public ZipCodeException
            (
            string? message,
            DomainExceptionTypeEnum type = DomainExceptionTypeEnum.BadInputData
            ) : base(message, type)
        {
        }
    }
}
