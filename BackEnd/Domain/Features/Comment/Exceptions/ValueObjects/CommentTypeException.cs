using Domain.Shared.Templates.Exceptions;

namespace Domain.Features.Comment.Exceptions.ValueObjects
{
    public class CommentTypeException : DomainException
    {
        public CommentTypeException
            (
            string? message,
            DomainExceptionTypeEnum type = DomainExceptionTypeEnum.BadInputData
            ) : base(message, type)
        {
        }
    }
}
