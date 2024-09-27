namespace Domain.Exceptions.UserExceptions.ValueObjectsExceptions
{
    public class ZipCodeException : Exception
    {
        public ZipCodeException() : base(Messages.InValidZipCode)
        {
        }
    }
}
