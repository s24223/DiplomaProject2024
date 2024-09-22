namespace Domain.Exceptions.UserExceptions
{
    public class CommentTypeException : Exception
    {
        public CommentTypeException(string? message) : base(message)
        {
        }
    }
}
