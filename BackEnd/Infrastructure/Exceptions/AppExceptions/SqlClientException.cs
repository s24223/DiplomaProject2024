namespace Infrastructure.Exceptions.AppExceptions
{
    public class SqlClientException : Exception
    {
        public SqlClientException(string? message) : base(message)
        {
        }
    }
}
