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
            value = value.Trim();

            if (!IsValidApartmentNumber(value))
            {
                throw new ApartmentNumberException(Messages.ApartmentNumber_Value_Invalid);
            }
            Value = value;
        }



        //====================================================================================================
        //====================================================================================================
        //====================================================================================================
        //Public Methods
        public static implicit operator ApartmentNumber?(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }
            else
            {
                return new ApartmentNumber(value);
            }
        }

        public static implicit operator string?(ApartmentNumber? value)
        {
            return value switch
            {
                null => null,
                _ => value.Value,
            };
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
