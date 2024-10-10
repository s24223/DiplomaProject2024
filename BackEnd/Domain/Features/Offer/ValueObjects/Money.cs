using Domain.Features.Offer.Exceptions.ValueObjects;

namespace Domain.Features.Offer.ValueObjects
{
    /// <summary>
    /// 
    /// </summary>
    /// <exception cref="MoneyException"></exception>
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
            if (decimal.Round(value, 2) != value || value < 0)
            {
                return false;
            }
            return true;
        }

        public static bool operator >(Money money1, Money money2)
        {
            return money1.Value > money2.Value;
        }

        public static bool operator <(Money money1, Money money2)
        {
            return money1.Value < money2.Value;
        }

        public static bool operator >=(Money money1, Money money2)
        {
            return money1.Value >= money2.Value;
        }

        public static bool operator <=(Money money1, Money money2)
        {
            return money1.Value <= money2.Value;
        }
    }
}
