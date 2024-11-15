using Domain.Shared.Templates.Exceptions;

namespace Domain.Features.Characteristic.Exceptions
{
    public class CharacteristicException : DomainException
    {
        public CharacteristicException(string? message, DomainExceptionTypeEnum type = DomainExceptionTypeEnum.BadInputData) : base(message, type)
        {
        }
    }
}
