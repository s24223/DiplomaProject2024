namespace Application.Shared.Exceptions.AppExceptions
{
    public class IncorrectJwtUserNameException : Exception
    {
        public IncorrectJwtUserNameException() : base(Messages.IncorrectJwtUserName)
        {
        }
    }
}
