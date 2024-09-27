using Domain.Exceptions.UserExceptions.ValueObjectsExceptions;
using System.Text.RegularExpressions;

namespace Domain.ValueObjects
{
    public record Regon
    {
        public string Value { get; private set; }

        public Regon(string value)
        {
            if (!IsValidRegon(value))
            {
                throw new RegonException(Messages.InValidRegon);
            }
            Value = value;
        }

        private bool IsValidRegon(string regon)
        {
            return Regex.IsMatch(regon, @"^[0-9]{9}$") ||
                Regex.IsMatch(regon, @"^[0-9]{14}$");

        }
    }
}
