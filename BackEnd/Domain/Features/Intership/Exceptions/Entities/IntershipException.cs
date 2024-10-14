using Domain.Shared.Templates.Exceptions;

namespace Domain.Features.Intership.Exceptions.Entities
{
    public class IntershipException : DomainException
    {
        public IntershipException
            (
            string? message,
            DomainExceptionTypeEnum type = DomainExceptionTypeEnum.BadInputData
            ) : base(message, type)
        {
        }
    }
}
