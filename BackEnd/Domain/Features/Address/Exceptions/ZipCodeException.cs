namespace Domain.Features.Address.Exceptions
{
    public class ZipCodeException : Exception
    {
        public ZipCodeException() : base(Messages.InValidZipCode)
        {
        }
    }
}
