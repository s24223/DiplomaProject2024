using Domain.Shared.Providers.ExceptionMessage;
using Domain.Shared.Providers.Time;

namespace Domain.Shared.Providers
{
    public interface IProvider
    {
        ITimeProvider TimeProvider();
        IExceptionMessageProvider ExceptionsMessageProvider();
    }
}
