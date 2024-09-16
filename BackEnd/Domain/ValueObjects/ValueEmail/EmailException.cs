namespace Domain.ValueObjects.ValueEmail
{
    public class EmailException : Exception
    {
        public EmailException(string? message) : base(message)
        {
        }
    }
}
