using Microsoft.Data.SqlClient;
using System.Reflection;

namespace Domain.Shared.Providers.ExceptionMessage
{
    public interface IExceptionMessageProvider
    {
        string GenerateExceptionMessage
             (
             Type classType,
             MethodBase? method,
             string? inputData = null,
             string? comment = null
             );

        //With Exception
        string GenerateExceptionMessage
            (
            Type classType,
            MethodBase? method,
            Exception ex,
            string? inputData = null,
            string? comment = null
            );

        string GenerateExceptionMessage
            (
            Type classType,
            MethodBase? method,
            SqlException ex,
            string? inputData = null,
            string? comment = null
            );
    }
}
