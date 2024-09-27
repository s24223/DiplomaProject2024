namespace Domain.Exceptions.UserExceptions.ValueObjectsExceptions
{
    public class UserProblemStatusException : Exception
    {
        public UserProblemStatusException(string? message) : base(message)
        {
        }
    }
}
