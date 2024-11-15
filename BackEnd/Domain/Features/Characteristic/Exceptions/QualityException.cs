using Domain.Shared.Templates.Exceptions;

namespace Domain.Features.Characteristic.Exceptions
{
    public class QualityException : DomainException
    {
        public QualityException(string? message, DomainExceptionTypeEnum type = DomainExceptionTypeEnum.BadInputData) : base(message, type)
        {
        }
    }
}
