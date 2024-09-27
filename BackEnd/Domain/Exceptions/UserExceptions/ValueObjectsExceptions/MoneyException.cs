namespace Domain.Exceptions.UserExceptions.ValueObjectsExceptions
{
    public class MoneyException : Exception
    {
        public MoneyException(string? message) : base(message)
        {
        }
    }
}
