namespace Infrastructure.Exceptions.AppExceptions
{
    public class InfrastructureLayerException : Exception
    {
        public InfrastructureLayerException(string? message) : base(message)
        {
        }
    }
}
