using System.Reflection;

namespace Application.Shared.Interfaces.Exceptions
{
    public interface IExceptionsRepository
    {
        Exception ConvertDbException
            (
            Exception ex,
            Type classType,
            MethodBase? method
            );
    }
}
