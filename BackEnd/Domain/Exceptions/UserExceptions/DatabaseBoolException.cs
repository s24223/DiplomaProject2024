namespace Domain.Exceptions.UserExceptions
{
    public class DatabaseBoolException : Exception
    {
        public DatabaseBoolException(string? message) : base(message)
        {
        }
    }
}
