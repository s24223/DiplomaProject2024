using Domain.Shared.Templates.Exceptions;

namespace Domain.Features.UserProblem.Exceptions.Entities
{
    public class UserProblemException : DomainException
    {
        public UserProblemException
            (
            string? message,
            DomainExceptionTypeEnum type = DomainExceptionTypeEnum.BadInputData
            ) : base(message, type)
        {
        }
    }
}
