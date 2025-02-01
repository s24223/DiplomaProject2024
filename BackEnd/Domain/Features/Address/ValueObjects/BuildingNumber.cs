using Domain.Features.Address.Exceptions.ValueObjects;
using System.Text.RegularExpressions;

namespace Domain.Features.Address.ValueObjects
{
    public record BuildingNumber
    {
        //Values
        public string Value { get; private set; }


        //Constructor
        public BuildingNumber(string value)
        {
            value = value.Trim();

            if (!IsValidBuildingNumber(value))
            {
                throw new BuildingNumberException(Messages.BuildingNumber_Value_Invalid);
            }
            Value = value;
        }


        //====================================================================================================
        //====================================================================================================
        //====================================================================================================
        //Public Methods
        public static implicit operator BuildingNumber(string value)
        {
            return new BuildingNumber(value);
        }

        public static implicit operator string(BuildingNumber value)
        {
            return value.Value;
        }

        //====================================================================================================
        //====================================================================================================
        //====================================================================================================
        //Private Methods
        private bool IsValidBuildingNumber(string value)
        {
            return Regex.IsMatch(value, @"^[1-9][0-9]*[a-zA-Z]?(/([1-9][0-9]*))?$");
        }
    }
}
