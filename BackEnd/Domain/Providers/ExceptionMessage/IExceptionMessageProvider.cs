using System.Reflection;

namespace Domain.Providers.ExceptionMessage
{
    public interface IExceptionMessageProvider
    {
        public string GenerateExceptionMessage
            (
            string message,
            Type classType,
            MethodBase? method
            );
        public string GenerateExceptionMessage
            (
            string message,
            Type classType,
            MethodBase? method,
            string inputData
            );
        public string GenerateExceptionMessage
            (
            string message,
            Type classType,
            MethodBase? method,
            string inputData,
            Exception exception
            );
        public string GenerateExceptionMessage
            (
            string message,
            Type classType,
            MethodBase? method,
            string inputData,
            string exceptionMessage
            );
    }
}
