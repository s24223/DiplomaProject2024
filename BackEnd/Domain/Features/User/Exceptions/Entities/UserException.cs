using Domain.Shared.Templates.Exceptions;

namespace Domain.Features.User.Exceptions.Entities
{
    public class UserException : DomainException
    {
        public UserException
            (
            string? message,
            DomainExceptionTypeEnum type = DomainExceptionTypeEnum.BadInputData
            ) : base(message, type)
        {
        }
    }
}
