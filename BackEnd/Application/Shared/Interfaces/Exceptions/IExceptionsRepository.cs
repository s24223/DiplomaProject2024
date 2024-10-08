namespace Application.Shared.Interfaces.Exceptions
{
    public interface IExceptionsRepository
    {
        Exception ConvertEFDbException
            (
            Exception ex
            );

        Exception ConvertSqlClientDbException
            (
            Exception ex,
            string? inputData = null
            );
    }
}
