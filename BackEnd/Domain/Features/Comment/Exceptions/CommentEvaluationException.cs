namespace Domain.Features.Comment.Exceptions
{
    public class CommentEvaluationException : Exception
    {
        public CommentEvaluationException(string? message) : base(message)
        {
        }
    }
}
