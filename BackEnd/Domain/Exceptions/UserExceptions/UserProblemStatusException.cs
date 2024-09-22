namespace Domain.Exceptions.UserExceptions
{
    public class UserProblemStatusException : Exception
    {
        public UserProblemStatusException(string? message) : base(message)
        {
        }
    }
}
