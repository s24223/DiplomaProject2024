using Domain.Features.Company.Exceptions.ValueObjects;
using System.Text.RegularExpressions;

namespace Domain.Features.Company.ValueObjects
{
    /// <summary>
    /// 
    /// </summary>
    /// <exception cref="RegonException"></exception>
    public record Regon
    {
        //Values
        public string Value { get; private set; }


        //Cosntructors
        public Regon(string value)
        {
            if (!IsValidRegon(value))
            {
                throw new RegonException(Messages.InValidRegon);
            }
            Value = value;
        }


        //==================================================================================================
        //==================================================================================================
        //==================================================================================================
        //Private Methods
        private bool IsValidRegon(string regon)
        {
            return Regex.IsMatch(regon, @"^[0-9]{9}$") ||
                Regex.IsMatch(regon, @"^[0-9]{14}$");

        }
    }
}
