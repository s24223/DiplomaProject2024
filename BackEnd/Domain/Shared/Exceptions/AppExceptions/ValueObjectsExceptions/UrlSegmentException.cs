namespace Domain.Shared.Exceptions.AppExceptions.ValueObjectsExceptions
{
    public class UrlSegmentException : Exception
    {
        public UrlSegmentException(string? message) : base(message)
        {
        }
    }
}
