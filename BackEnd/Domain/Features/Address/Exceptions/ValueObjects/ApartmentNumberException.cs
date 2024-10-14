using Domain.Shared.Templates.Exceptions;

namespace Domain.Features.Address.Exceptions.ValueObjects
{
    public class ApartmentNumberException : DomainException
    {
        public ApartmentNumberException
            (
            string? message,
            DomainExceptionTypeEnum type = DomainExceptionTypeEnum.BadInputData
            ) : base(message, type)
        {
        }
    }
}
