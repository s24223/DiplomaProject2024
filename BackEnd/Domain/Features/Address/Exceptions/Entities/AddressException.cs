using Domain.Shared.Templates.Exceptions;

namespace Domain.Features.Address.Exceptions.Entities
{
    public class AddressException : DomainException
    {
        public AddressException
            (
            string? message,
            DomainExceptionTypeEnum type = DomainExceptionTypeEnum.BadInputData
            ) : base(message, type)
        {
        }
    }
}
