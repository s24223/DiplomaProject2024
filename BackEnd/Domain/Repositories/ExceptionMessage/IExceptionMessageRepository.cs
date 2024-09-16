using System.Reflection;

namespace Domain.Repositories.ExceptionMessage
{
    public interface IExceptionMessageRepository
    {
        public string GenerateExceptionMessage(string message, Type classType, MethodBase? method);
        public string GenerateExceptionMessage(string message, Type classType, MethodBase? method, string inputData);
    }
}
