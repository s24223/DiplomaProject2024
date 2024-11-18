using Domain.Features.Person.Entities;
using Domain.Features.User.ValueObjects.Identificators;

namespace Application.Features.Persons.Commands.Interfaces
{
    public interface IPersonCmdRepository
    {
        //DML
        Task<DomainPerson> CreateAsync(DomainPerson person, CancellationToken cancellation);
        Task<DomainPerson> UpdateAsync(DomainPerson person, CancellationToken cancellation);

        //DQL
        Task<DomainPerson> GetPersonAsync(UserId id, CancellationToken cancellation);
    }
}
