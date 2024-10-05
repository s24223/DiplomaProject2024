using Domain.Exceptions.AppExceptions.ValueObjectsExceptions.ValueObjectsExceptions;
using System.Text.RegularExpressions;

namespace Domain.ValueObjects.AddressPart
{
    public record ZipCode
    {
        public string Value { get; private set; } = null!;

        public ZipCode(string value)
        {
            if (!IsvalidZipCode(value))
            {
                throw new ZipCodeException();
            }
            Value = value;
        }

        private bool IsvalidZipCode(string value)
        {
            return Regex.IsMatch(value, @"^[0-9]{5}$");
        }
    }
}
