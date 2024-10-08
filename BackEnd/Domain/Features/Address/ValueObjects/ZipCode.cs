using Domain.Features.Address.Exceptions.ValueObjects;
using System.Text.RegularExpressions;

namespace Domain.Features.Address.ValueObjects
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
