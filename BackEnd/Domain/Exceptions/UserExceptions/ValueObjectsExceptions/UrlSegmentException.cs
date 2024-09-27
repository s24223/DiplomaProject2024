namespace Domain.Exceptions.UserExceptions.ValueObjectsExceptions
{
    public class UrlSegmentException : Exception
    {
        public UrlSegmentException(string? message) : base(message)
        {
        }
    }
}
