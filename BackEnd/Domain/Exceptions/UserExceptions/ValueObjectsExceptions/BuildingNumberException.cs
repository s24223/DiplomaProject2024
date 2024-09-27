namespace Domain.Exceptions.UserExceptions.ValueObjectsExceptions
{
    public class BuildingNumberException : Exception
    {
        public BuildingNumberException() : base(Messages.InValidBuildingNumber)
        {
        }
    }
}
