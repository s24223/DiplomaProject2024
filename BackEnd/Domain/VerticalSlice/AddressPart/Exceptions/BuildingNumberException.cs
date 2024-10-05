namespace Domain.VerticalSlice.AddressPart.Exceptions
{
    public class BuildingNumberException : Exception
    {
        public BuildingNumberException() : base(Messages.InValidBuildingNumber)
        {
        }
    }
}
