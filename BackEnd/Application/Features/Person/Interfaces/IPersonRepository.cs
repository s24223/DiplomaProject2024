using Domain.Features.Person.Entities;

namespace Application.Features.Person.Interfaces
{
    public interface IPersonRepository
    {
        Task CreatePersonProfileAsync(DomainPerson person, CancellationToken cancellation);
    }
}
