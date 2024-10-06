namespace Domain.Features.Address.Exceptions
{
    public class BuildingNumberException : Exception
    {
        public BuildingNumberException() : base(Messages.InValidBuildingNumber)
        {
        }
    }
}
