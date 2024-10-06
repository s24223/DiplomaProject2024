namespace Domain.Features.Person.Exceptions
{
    public class PhoneNumberException : Exception
    {
        public PhoneNumberException(string? message) : base(message)
        {
        }
    }
}
