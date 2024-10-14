using Domain.Features.Address.Exceptions.ValueObjects;
using System.Text.RegularExpressions;

namespace Domain.Features.Address.ValueObjects
{
    public record ApartmentNumber
    {
        //Values
        public string Value { get; private set; }


        //Cosntructor
        public ApartmentNumber(string value)
        {
            if (!IsValidApartmentNumber(value))
            {
                throw new ApartmentNumberException(Messages.ApartmentNumber_Value_Invalid);
            }
            Value = value;
        }


        //====================================================================================================
        //====================================================================================================
        //====================================================================================================
        //Private Methods
        private bool IsValidApartmentNumber(string value)
        {
            return Regex.IsMatch(value, @"^[1-9][0-9]*$");
        }
    }
}
