namespace Application.Shared.Exceptions.UserExceptions
{
    public class UnauthorizedUserException : Exception
    {
        public UnauthorizedUserException() : base(Messages.ExeptionMessageUnauthorizedUser)
        {
        }

    }
}
