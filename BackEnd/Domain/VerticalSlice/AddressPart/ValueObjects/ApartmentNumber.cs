using Domain.VerticalSlice.AddressPart.Exceptions;
using System.Text.RegularExpressions;

namespace Domain.VerticalSlice.AddressPart.ValueObjects
{
    public record ApartmentNumber
    {
        public string Value { get; private set; }

        public ApartmentNumber(string value)
        {
            if (!IsValidApartmentNumber(value))
            {
                throw new ApartmentNumberException();
            }
            Value = value;
        }

        private bool IsValidApartmentNumber(string value)
        {
            return Regex.IsMatch(value, @"^[1-9][0-9]*$");
        }
    }
}
