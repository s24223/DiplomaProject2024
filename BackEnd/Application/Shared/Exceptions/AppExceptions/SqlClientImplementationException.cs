namespace Application.Shared.Exceptions.AppExceptions
{
    public class SqlClientImplementationException : Exception
    {
        public SqlClientImplementationException(string? message) : base(message)
        {
        }
    }
}
