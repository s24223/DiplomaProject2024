namespace Domain.Exceptions.UserExceptions.ValueObjectsExceptions
{
    public class DatabaseBoolException : Exception
    {
        public DatabaseBoolException(string? message) : base(message)
        {
        }
    }
}
