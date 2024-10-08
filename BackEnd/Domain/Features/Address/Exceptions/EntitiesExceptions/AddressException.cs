using Domain.Shared.Templates.Exceptions;

namespace Domain.Features.Address.Exceptions.EntitiesExceptions
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
