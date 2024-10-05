using Domain.VerticalSlice.PersonPart.Entities;

namespace Application.VerticalSlice.PersonPart.Interfaces
{
    public interface IPersonRepository
    {
        Task CreatePersonProfileAsync(DomainPerson person, CancellationToken cancellation);
    }
}
