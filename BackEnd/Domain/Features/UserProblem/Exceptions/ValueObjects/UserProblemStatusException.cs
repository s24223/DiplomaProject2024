using Domain.Shared.Templates.Exceptions;

namespace Domain.Features.UserProblem.Exceptions.ValueObjects
{
    public class UserProblemStatusException : DomainException
    {
        public UserProblemStatusException
            (
            string? message,
            DomainExceptionTypeEnum type = DomainExceptionTypeEnum.BadInputData
            ) : base(message, type)
        {
        }
    }
}
