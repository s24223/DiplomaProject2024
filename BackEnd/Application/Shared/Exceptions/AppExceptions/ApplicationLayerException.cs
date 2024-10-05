namespace Application.Shared.Exceptions.AppExceptions
{
    public class ApplicationLayerException : Exception
    {
        public ApplicationLayerException(string? message) : base(message)
        {
        }
    }
}
