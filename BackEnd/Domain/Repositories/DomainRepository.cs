using Domain.Repositories.ExceptionMessage;
using Domain.Repositories.Time;

namespace Domain.Repositories
{
    public class DomainRepository : IDomainRepository
    {
        private readonly IExceptionMessageRepository _exceptionMessageRepository;
        private readonly ITimeRepository _timeRepository;

        public DomainRepository
            (
            IExceptionMessageRepository exceptionMessageRepository,
            ITimeRepository timeRepository
            )
        {
            _exceptionMessageRepository = exceptionMessageRepository;
            _timeRepository = timeRepository;
        }

        public IExceptionMessageRepository GetExceptionMessageRepository() => _exceptionMessageRepository;

        public ITimeRepository GetTimeRepository() => _timeRepository;
    }
}
