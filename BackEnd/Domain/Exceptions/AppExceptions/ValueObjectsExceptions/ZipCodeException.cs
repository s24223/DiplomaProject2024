namespace Domain.Exceptions.AppExceptions.ValueObjectsExceptions.ValueObjectsExceptions
{
    public class ZipCodeException : Exception
    {
        public ZipCodeException() : base(Messages.InValidZipCode)
        {
        }
    }
}
