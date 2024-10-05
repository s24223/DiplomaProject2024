namespace Infrastructure.Exceptions.AppExceptions
{
    public class InfrastructureException : Exception
    {
        public InfrastructureException(string? message) : base(message)
        {
        }
    }
}
