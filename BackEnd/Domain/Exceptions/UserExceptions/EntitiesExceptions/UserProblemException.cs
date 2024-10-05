namespace Domain.Exceptions.UserExceptions.EntitiesExceptions
{
    public class UserProblemException : Exception
    {
        public UserProblemException(string? message) : base(message)
        {
        }
    }
}
