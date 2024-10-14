using Domain.Shared.Templates.Exceptions;

namespace Application.Shared.Exceptions
{
    public class DateOnlyException : DomainException
    {
        public DateOnlyException
            (
            string? message,
            DomainExceptionTypeEnum type = DomainExceptionTypeEnum.BadInputData
            ) : base(message, type)
        {
        }
    }
}
