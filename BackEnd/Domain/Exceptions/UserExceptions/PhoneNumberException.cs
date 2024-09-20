namespace Domain.Exceptions.UserExceptions
{
    public class PhoneNumberException : Exception
    {
        public PhoneNumberException(string? message) : base(message)
        {
        }
    }
}
