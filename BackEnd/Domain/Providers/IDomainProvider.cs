using Domain.Providers.ExceptionMessage;
using Domain.Providers.Time;

namespace Domain.Providers
{
    public interface IDomainProvider
    {
        ITimeProvider GetTimeProvider();
        IExceptionMessageProvider GetExceptionsMessageProvider();
    }
}
