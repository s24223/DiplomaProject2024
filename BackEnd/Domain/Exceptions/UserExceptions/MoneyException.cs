namespace Domain.Exceptions.UserExceptions
{
    public class MoneyException : Exception
    {
        public MoneyException(string? message) : base(message)
        {
        }
    }
}
