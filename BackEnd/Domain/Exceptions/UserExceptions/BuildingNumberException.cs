namespace Domain.Exceptions.UserExceptions
{
    public class BuildingNumberException : Exception
    {
        public BuildingNumberException() : base(Messages.InValidBuildingNumber)
        {
        }
    }
}
