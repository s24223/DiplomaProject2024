using Domain.Shared.Providers.ExceptionMessage;
using Domain.Shared.Providers.Time;

namespace Domain.Shared.Providers
{
    public class Provider : IProvider
    {
        private readonly IExceptionMessageProvider _exceptionMessageProvider;
        private readonly ITimeProvider _timeProvider;

        public Provider
            (
            IExceptionMessageProvider exceptionMessageProvider,
            ITimeProvider timeProvider
            )
        {
            _exceptionMessageProvider = exceptionMessageProvider;
            _timeProvider = timeProvider;
        }

        public IExceptionMessageProvider ExceptionsMessageProvider() => _exceptionMessageProvider;

        public ITimeProvider TimeProvider() => _timeProvider;
    }
}
