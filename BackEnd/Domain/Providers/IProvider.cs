using Domain.Providers.ExceptionMessage;
using Domain.Providers.Time;

namespace Domain.Providers
{
    public interface IProvider
    {
        ITimeProvider TimeProvider();
        IExceptionMessageProvider ExceptionsMessageProvider();
    }
}
