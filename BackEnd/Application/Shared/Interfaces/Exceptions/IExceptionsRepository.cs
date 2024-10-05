using System.Reflection;

namespace Application.Shared.Interfaces.Exceptions
{
    public interface IExceptionsRepository
    {
        Exception ConvertEFDbException
            (
            Exception ex,
            Type classType,
            MethodBase? method
            );

        Exception ConvertSqlClientDbException
            (
            Exception ex,
            string? inputData = null
            );
    }
}
