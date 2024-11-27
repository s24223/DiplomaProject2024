using Domain.Features.Person.Entities;
using Domain.Features.User.ValueObjects.Identificators;

namespace Application.Features.Persons.Commands.Interfaces
{
    public interface IPersonCmdRepo
    {
        //DML
        Task<DomainPerson> CreatePersonAsync(DomainPerson person, CancellationToken cancellation);
        Task<DomainPerson> UpdatePersonAsync(DomainPerson person, CancellationToken cancellation);

        //DQL
        Task<DomainPerson> GetPersonAsync(UserId id, CancellationToken cancellation);
    }
}
