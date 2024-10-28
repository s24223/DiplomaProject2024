using Domain.Features.Company.Exceptions.ValueObjects;
using System.Text.RegularExpressions;

namespace Domain.Features.Company.ValueObjects
{
    public record Regon
    {
        //Values
        public string Value { get; private set; }


        //Cosntructors
        public Regon(string value)
        {
            value = value.Trim();

            if (!IsValidRegon(value))
            {
                throw new RegonException(Messages.Regon_Value_Invalid);
            }
            Value = value;
        }


        //==================================================================================================
        //==================================================================================================
        //==================================================================================================
        //Public Methods
        public static implicit operator Regon(string value)
        {
            return new Regon(value);
        }

        public static implicit operator string(Regon value)
        {
            return value.Value;
        }

        //==================================================================================================
        //==================================================================================================
        //==================================================================================================
        //Private Methods
        private bool IsValidRegon(string regon)
        {
            return Regex.IsMatch(regon, @"^[0-9]{9}$") ||
                Regex.IsMatch(regon, @"^[0-9]{14}$");

        }
    }
}
