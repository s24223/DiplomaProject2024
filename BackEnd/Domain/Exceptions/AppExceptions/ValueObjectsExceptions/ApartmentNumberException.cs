namespace Domain.Exceptions.AppExceptions.ValueObjectsExceptions.ValueObjectsExceptions
{
    public class ApartmentNumberException : Exception
    {
        public ApartmentNumberException() : base(Messages.InValidApartmentNumber)
        {
        }
    }
}
