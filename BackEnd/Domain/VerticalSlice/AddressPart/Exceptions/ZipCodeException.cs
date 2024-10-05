namespace Domain.VerticalSlice.AddressPart.Exceptions
{
    public class ZipCodeException : Exception
    {
        public ZipCodeException() : base(Messages.InValidZipCode)
        {
        }
    }
}
