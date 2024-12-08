using Domain.Shared.Templates.Exceptions;

namespace Domain.Features.Comment.Exceptions.Entities
{
    public class CommentException : DomainException
    {
        public CommentException
            (string? message, DomainExceptionTypeEnum type = DomainExceptionTypeEnum.BadInputData) :
            base(message, type)
        {
        }
    }
}
