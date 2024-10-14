using Domain.Features.Address.Exceptions.ValueObjects;
using System.Text.RegularExpressions;

namespace Domain.Features.Address.ValueObjects
{
    public record BuildingNumber
    {
        //Values
        public string Value { get; private set; }


        //Cosntructor
        public BuildingNumber(string value)
        {
            if (!IsValidBuildingNumber(value))
            {
                throw new BuildingNumberException(Messages.BuildingNumber_Value_Invalid);
            }
            Value = value;
        }


        //====================================================================================================
        //====================================================================================================
        //====================================================================================================
        //Private Methods
        private bool IsValidBuildingNumber(string value)
        {
            return Regex.IsMatch(value, @"^[1-9][0-9]*[a-zA-Z]?[/]?[1-9]?[0-9]*$");
        }
    }
}
