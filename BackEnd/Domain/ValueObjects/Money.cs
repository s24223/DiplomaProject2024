using Domain.Exceptions.UserExceptions;

namespace Domain.ValueObjects
{
    public record Money
    {
        public decimal Value { get; private set; }

        public Money(decimal value)
        {
            if (!IsValidMoney(value))
            {
                throw new MoneyException(Messages.InValidMoney);
            }
            Value = value;
        }

        private bool IsValidMoney(decimal value)
        {
            if (Decimal.Round(value, 2) != value || value < 0)
            {
                return false;
            }
            return true;
        }
    }
}
