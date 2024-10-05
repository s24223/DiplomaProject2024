using Domain.VerticalSlice.AddressPart.Exceptions;
using System.Text.RegularExpressions;

namespace Domain.VerticalSlice.AddressPart.ValueObjects
{
    public record BuildingNumber
    {
        public string Value { get; private set; }

        public BuildingNumber(string value)
        {
            if (IsValidBuildingNumber(value))
            {
                throw new BuildingNumberException();
            }
            Value = value;
        }

        private bool IsValidBuildingNumber(string value)
        {
            return Regex.IsMatch(value, @"^[1-9][0-9]*[a-zA-Z]?[/]?[1-9]?[0-9]*$");
        }
    }
}
