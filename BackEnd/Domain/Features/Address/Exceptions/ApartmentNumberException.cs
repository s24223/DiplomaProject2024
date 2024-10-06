namespace Domain.Features.Address.Exceptions
{
    public class ApartmentNumberException : Exception
    {
        public ApartmentNumberException() : base(Messages.InValidApartmentNumber)
        {
        }
    }
}
