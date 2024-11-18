using Domain.Features.Person.Exceptions.ValueObjects;
using System.Text.RegularExpressions;

namespace Domain.Features.Person.ValueObjects
{
    public record PhoneNumber
    {
        //Values
        public string Value { get; private set; } = null!;


        //Cosntructor
        public PhoneNumber(string value)
        {
            if (!IsValidPhoneNumber(value))
            {
                throw new PhoneNumberException(Messages.PhoneNumber_Value_Invalid);
            }
            Value = value;
        }

        //==================================================================================================
        //==================================================================================================
        //==================================================================================================
        //Public Methods
        public static explicit operator string?(PhoneNumber? phoneNumber) => phoneNumber?.Value;

        //==================================================================================================
        //==================================================================================================
        //==================================================================================================
        //Private Methods
        private bool IsValidPhoneNumber(string value)
        {
            return Regex.IsMatch(value, @"^[0-9]{9}$");
        }
    }
}
