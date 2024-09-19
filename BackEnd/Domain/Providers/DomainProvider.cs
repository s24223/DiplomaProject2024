using Domain.Providers.ExceptionMessage;
using Domain.Providers.Time;

namespace Domain.Providers
{
    public class DomainProvider : IDomainProvider
    {
        private readonly IExceptionMessageProvider _exceptionMessageRepository;
        private readonly ITimeProvider _timeRepository;

        public DomainProvider
            (
            IExceptionMessageProvider exceptionMessageRepository,
            ITimeProvider timeRepository
            )
        {
            _exceptionMessageRepository = exceptionMessageRepository;
            _timeRepository = timeRepository;
        }

        public IExceptionMessageProvider GetExceptionsMessageProvider() => _exceptionMessageRepository;

        public ITimeProvider GetTimeProvider() => _timeRepository;
    }
}
