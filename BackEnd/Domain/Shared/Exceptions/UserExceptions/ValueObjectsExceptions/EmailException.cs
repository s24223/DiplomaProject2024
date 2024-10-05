namespace Domain.Shared.Exceptions.UserExceptions.ValueObjectsExceptions
{
    public class EmailException : Exception
    {
        public EmailException(string? message) : base(message)
        {
        }
    }
}
