using Domain.Shared.Exceptions.ValueObjects;
using System.Text.RegularExpressions;

namespace Domain.Shared.ValueObjects
{
    /// <summary>
    /// 
    /// </summary>
    /// <exception cref="EmailException"></exception>
    public record Email
    {
        public string Value { get; private set; } = null!;

        public Email(string value)
        {
            if (!IsValidEmail(value))
            {
                throw new EmailException(Messages.Email_Value_Invalid);
            }
            Value = value;
        }

        private bool IsValidEmail(string str)
        {
            // Return true if strIn is in valid e-mail format.
            return Regex.IsMatch(str, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }
    }
}
