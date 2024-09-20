using Domain.Exceptions.UserExceptions;
using System.Text.RegularExpressions;

namespace Domain.ValueObjects
{
    public record PhoneNumber
    {
        public string Value { get; private set; } = null!;

        public PhoneNumber(string value)
        {
            if (!IsValidPhoneNumber(value))
            {
                throw new PhoneNumberException(Messages.InValidPhoneNumber);
            }
            Value = value;
        }

        private bool IsValidPhoneNumber(string value)
        {
            return Regex.IsMatch(value, @"^[0-9]{9}$");
        }
    }
}
