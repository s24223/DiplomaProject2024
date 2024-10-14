using Domain.Features.Person.Entities;
using Domain.Features.User.ValueObjects.Identificators;

namespace Application.Features.Person.Interfaces
{
    public interface IPersonRepository
    {
        //DML
        Task CreateAsync(DomainPerson person, CancellationToken cancellation);
        Task UpdateAsync(DomainPerson person, CancellationToken cancellation);

        //DQL
        Task<DomainPerson> GetPersonAsync(UserId id, CancellationToken cancellation);
    }
}
