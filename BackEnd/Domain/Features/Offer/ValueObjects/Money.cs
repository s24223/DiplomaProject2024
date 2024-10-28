using Domain.Features.Offer.Exceptions.ValueObjects;

namespace Domain.Features.Offer.ValueObjects
{
    public record Money
    {
        //Values
        public decimal Value { get; private set; }


        //Cosntructor
        public Money(decimal value)
        {
            if (!IsValidMoney(value))
            {
                throw new MoneyException(Messages.Money_Value_Invalid);
            }
            Value = value;
        }


        //================================================================================================
        //================================================================================================
        //================================================================================================
        //Public Methods
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

        public static implicit operator decimal?(Money? money)
        {
            return money switch
            {
                null => null,
                _ => money.Value,
            };
        }

        public static implicit operator Money?(decimal? value)
        {
            return value switch
            {
                null => null,
                _ => new Money(value.Value),
            };
        }

        //================================================================================================
        //================================================================================================
        //================================================================================================
        //Private Methods
        private bool IsValidMoney(decimal value)
        {
            if (decimal.Round(value, 2) != value || value < 0)
            {
                return false;
            }
            return true;
        }

    }
}
