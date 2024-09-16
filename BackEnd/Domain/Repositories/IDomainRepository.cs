using Domain.Repositories.ExceptionMessage;
using Domain.Repositories.Time;

namespace Domain.Repositories
{
    public interface IDomainRepository
    {
        ITimeRepository GetTimeRepository();
        IExceptionMessageRepository GetExceptionMessageRepository();
    }
}
