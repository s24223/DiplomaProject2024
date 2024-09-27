namespace Domain.Exceptions.UserExceptions.ValueObjectsExceptions
{
    public class ApartmentNumberException : Exception
    {
        public ApartmentNumberException() : base(Messages.InValidApartmentNumber)
        {
        }
    }
}
