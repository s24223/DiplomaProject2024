using Domain.Shared.Templates.Exceptions;

namespace Domain.Features.Comment.Exceptions.ValueObjects
{
    public class CommentEvaluationException : DomainException
    {
        public CommentEvaluationException
            (
            string? message,
            DomainExceptionTypeEnum type = DomainExceptionTypeEnum.BadInputData
            ) : base(message, type)
        {
        }
    }
}
