using Domain.Exceptions.AppExceptions.ValueObjectsExceptions.ValueObjectsExceptions;

namespace Domain.ValueObjects
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
            if (Decimal.Round(value, 2) != value || value < 0)
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
