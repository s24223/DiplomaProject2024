namespace Domain.Features.Comment.Exceptions
{
    public class CommentTypeException : Exception
    {
        public CommentTypeException(string? message) : base(message)
        {
        }
    }
}
