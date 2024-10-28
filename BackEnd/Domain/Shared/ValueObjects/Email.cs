using Domain.Shared.Exceptions.ValueObjects;
using System.Text.RegularExpressions;

namespace Domain.Shared.ValueObjects
{
    public record Email
    {
        //Values
        public string Value { get; private set; } = null!;


        //Cosntructor
        public Email(string value)
        {
            value = value.Trim();

            if (!IsValidEmail(value))
            {
                throw new EmailException(Messages.Email_Value_Invalid);
            }
            Value = value;
        }


        //=====================================================================================================================
        //=====================================================================================================================
        //=====================================================================================================================
        //Public Methods
        public static implicit operator Email(string value)
        {
            return new Email(value);
        }
        public static implicit operator string(Email value)
        {
            return value.Value;
        }

        //=====================================================================================================================
        //=====================================================================================================================
        //=====================================================================================================================
        //Private Methods
        private bool IsValidEmail(string str)
        {
            // Return true if strIn is in valid e-mail format.
            return Regex.IsMatch(str, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }
    }
}
