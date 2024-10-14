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
        //Private Methods
        private bool IsvalidZipCode(string value)
        {
            return Regex.IsMatch(value, @"^[0-9]{5}$");
        }
    }
}
