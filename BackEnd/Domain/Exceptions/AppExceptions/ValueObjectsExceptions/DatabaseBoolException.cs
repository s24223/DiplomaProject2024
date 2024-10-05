namespace Domain.Exceptions.AppExceptions.ValueObjectsExceptions
{
    public class DatabaseBoolException : Exception
    {
        public DatabaseBoolException(string? message) : base(message)
        {
        }
    }
}
