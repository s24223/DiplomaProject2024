using Domain.Features.Address.Exceptions.ValueObjects;
using System.Text.RegularExpressions;

namespace Domain.Features.Address.ValueObjects
{
    public record ZipCode
    {
        //Values
        public string Value { get; private set; } = null!;


        //Cosntructor
        public ZipCode(string value)
        {
            if (!IsvalidZipCode(value))
            {
                throw new ZipCodeException(Messages.ZipCode_Value_Invalid);
            }
            Value = value;
        }


        //====================================================================================================
        //====================================================================================================
        //====================================================================================================
        //Public Methods
        public static implicit operator ZipCode(string value)
        {
            return new ZipCode(value);
        }

        public static implicit operator string(ZipCode value)
        {
            return value.Value;
        }
        //====================================================================================================
        //====================================================================================================
        //====================================================================================================
        //Private Methods
        private bool IsvalidZipCode(string value)
        {
            return Regex.IsMatch(value, @"^[0-9]{5}$");
        }
    }
}
