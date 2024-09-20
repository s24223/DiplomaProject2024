namespace Domain.Exceptions.UserExceptions
{
    internal class UrlException : Exception
    {
        public UrlException(string? message) : base(message)
        {
        }
    }
}
