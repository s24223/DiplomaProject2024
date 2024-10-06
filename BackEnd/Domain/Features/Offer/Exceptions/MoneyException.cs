namespace Domain.Features.Offer.Exceptions
{
    public class MoneyException : Exception
    {
        public MoneyException(string? message) : base(message)
        {
        }
    }
}
