using System.Reflection;

namespace Domain.Providers.ExceptionMessage
{
    public class ExceptionMessageProvider : IExceptionMessageProvider
    {
        private readonly string _message = "Message";
        private readonly string _class = "Class";
        private readonly string _method = "Method";
        private readonly string _inputData = "Input Data";

        public string GenerateExceptionMessage
            (
            string message,
            Type classType,
            MethodBase? method,
            string inputData
            )
        {
            var className = classType.FullName;
            if (method != null)
            {
                var methodName = method.Name;
                return $"{_message}: {message}; {_class}: {className}; {_method}: {methodName}; {_inputData}: {inputData};";
            }
            else
            {
                return $"{_message}: {message}; {_class}: {className}; {_inputData}: {inputData};";
            }
        }

        public string GenerateExceptionMessage
            (
            string message,
            Type classType,
            MethodBase? method
            )
        {
            var className = classType.FullName;
            if (method != null)
            {
                var methodName = method.Name;
                return $"{_message}: {message}; {_class}: {className}; {_method}: {methodName}; ";
            }
            else
            {
                return $"{_message}: {message}; {_class}: {className};";
            }
        }

        public string GenerateExceptionMessage
            (
            string message,
            Type classType,
            MethodBase? method,
            string inputData,
            Exception exception
            )
        {
            return $"{GenerateExceptionMessage(message, classType, method, inputData)}; Exception Message: {exception.Message};";
        }

        public string GenerateExceptionMessage
            (
            string message,
            Type classType,
            MethodBase? method,
            string inputData,
            string exceptionMessage
            )
        {
            return $"{GenerateExceptionMessage(message, classType, method, inputData)}; Exception Message: {exceptionMessage};";
        }
    }
}
