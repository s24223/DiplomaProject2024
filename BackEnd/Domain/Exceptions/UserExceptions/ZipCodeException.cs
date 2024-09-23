namespace Domain.Exceptions.UserExceptions
{
    public class ZipCodeException : Exception
    {
        public ZipCodeException() : base(Messages.InValidZipCode)
        {
        }
    }
}
