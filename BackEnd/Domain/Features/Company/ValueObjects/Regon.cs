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
            if (!IsValidRegon(value))
            {
                throw new RegonException(Messages.Regon_Value_Invalid);
            }
            Value = value;
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
