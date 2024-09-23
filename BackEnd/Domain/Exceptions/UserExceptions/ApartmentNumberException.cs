namespace Domain.Exceptions.UserExceptions
{
    public class ApartmentNumberException : Exception
    {
        public ApartmentNumberException() : base(Messages.InValidApartmentNumber)
        {
        }
    }
}
