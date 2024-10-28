using Domain.Features.User.Entities;
using Domain.Features.User.ValueObjects.Identificators;

namespace Application.Features.User.Interfaces.QueriesUser
{
    public interface IUserQueryRepository
    {
        Task<DomainUser> GetUserDataAsync(UserId id, CancellationToken cancellation);
    }
}
