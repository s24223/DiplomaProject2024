using Microsoft.Data.SqlClient;
using System.Reflection;
using System.Text;

namespace Domain.Shared.Providers.ExceptionMessage
{
    public class ExceptionMessageProvider : IExceptionMessageProvider
    {
        private readonly string _class = "Class:";
        private readonly string _method = "Method:";
        private readonly string _inputData = "Input Data:";
        private readonly string _comment = "Comment:";
        private readonly string _exceptionType = "Exception Type:";
        private readonly string _exceptionMessage = "Exception Message:";
        private readonly string _sqlExceptionNumber = "SqlException Number:";

        public string GenerateExceptionMessage
             (
             Type classType,
             MethodBase? method,
             string? inputData = null,
             string? comment = null
             )
        {
            var className = classType.FullName;
            var methodName = method == null ? null : method.Name;

            var message = methodName == null ?
                $"{_class} {className};" :
                 $"{_class} {className}; {_method} {methodName};";

            if (!string.IsNullOrWhiteSpace(inputData))
            {
                message = $"{message} {_inputData} {inputData};";
            }
            if (!string.IsNullOrWhiteSpace(comment))
            {
                message = $"{message} {_comment} {comment};";
            }
            return message;
        }


        public string GenerateExceptionMessage
            (
            Type classType,
            MethodBase? method,
            Exception ex,
            string? inputData = null,
            string? comment = null
            )
        {
            var className = classType.FullName;
            var methodName = method == null ? null : method.Name;

            var message = methodName == null ?
                $"{_class} {className};" :
                 $"{_class} {className}; {_method} {methodName};";

            message = $"{message} {_exceptionType} {ex.GetType().Name}; {_exceptionMessage} {ex.Message};";

            if (!string.IsNullOrWhiteSpace(inputData))
            {
                message = $"{message} {_inputData} {inputData};";
            }
            if (!string.IsNullOrWhiteSpace(comment))
            {
                message = $"{message} {_comment} {comment};";
            }
            return message;
        }

        public string GenerateExceptionMessage
            (
            Type classType,
            MethodBase? method,
            SqlException ex,
            string? inputData = null,
            string? comment = null
            )
        {
            var className = classType.FullName;
            var methodName = method == null ? null : method.Name;

            var message = methodName == null ?
                $"{_class} {className};" :
                 $"{_class} {className}; {_method} {methodName};";

            var x = ex.Number;
            message = string.Format
                (
                "{0} {1} {2}; {3} {4}; {5} {6};",
                message,
                _exceptionType,
                ex.GetType().Name,
                _sqlExceptionNumber,
                ex.Number.ToString(),
                _exceptionMessage,
                PrintSqlExceptionsErrors(ex)
                );

            if (!string.IsNullOrWhiteSpace(inputData))
            {
                message = $"{message} {_inputData} {inputData};";
            }
            if (!string.IsNullOrWhiteSpace(comment))
            {
                message = $"{message} {_comment} {comment};";
            }
            return message;
        }

        //=====================================================================================================
        //=====================================================================================================
        //=====================================================================================================
        //Private Methods
        private string PrintSqlExceptionsErrors(SqlException ex)
        {
            /* https://learn.microsoft.com/pl-pl/dotnet/api/system.data.sqlclient.sqlexception?view=dotnet-plat-ext-8.0
                    */
            StringBuilder errorMessages = new StringBuilder();
            for (int i = 0; i < ex.Errors.Count; i++)
            {
                errorMessages.Append("Index #" + i + "\n" +
                    "Message: " + ex.Errors[i].Message + "\n" +
                    "LineNumber: " + ex.Errors[i].LineNumber + "\n" +
                    "Source: " + ex.Errors[i].Source + "\n" +
                    "Procedure: " + ex.Errors[i].Procedure + "\n");
            }
            return errorMessages.ToString();
        }
    }
}
