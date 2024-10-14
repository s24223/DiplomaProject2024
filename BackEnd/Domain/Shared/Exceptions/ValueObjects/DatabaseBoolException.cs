using Domain.Shared.Templates.Exceptions;

namespace Domain.Shared.Exceptions.ValueObjects
{
    public class DatabaseBoolException : DomainException
    {
        public DatabaseBoolException
            (
            string? message,
            DomainExceptionTypeEnum type = DomainExceptionTypeEnum.AppProblem
            ) : base(message, type)
        {
        }
    }
}
