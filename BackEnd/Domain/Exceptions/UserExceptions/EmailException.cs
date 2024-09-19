namespace Domain.Exceptions.UserExceptions
{
    public class EmailException : Exception
    {
        public EmailException(string? message) : base(message)
        {
        }
    }
}
