namespace Domain.VerticalSlice.AddressPart.Exceptions
{
    public class ApartmentNumberException : Exception
    {
        public ApartmentNumberException() : base(Messages.InValidApartmentNumber)
        {
        }
    }
}
