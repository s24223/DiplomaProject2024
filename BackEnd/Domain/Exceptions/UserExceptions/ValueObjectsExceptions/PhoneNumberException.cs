namespace Domain.Exceptions.UserExceptions.ValueObjectsExceptions
{
    public class PhoneNumberException : Exception
    {
        public PhoneNumberException(string? message) : base(message)
        {
        }
    }
}
