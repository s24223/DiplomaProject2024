﻿using Domain.Features.Person.Exceptions;
using System.Text.RegularExpressions;

namespace Domain.Features.Person.ValueObjects
{
    /// <summary>
    /// 
    /// </summary>
    /// <exception cref="PhoneNumberException"></exception>
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
