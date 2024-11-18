using Application.Databases.Relational.Models;
using Domain.Features.Person.Entities;

namespace Application.Features.Persons.Mappers
{
    public interface IPersonMapper
    {
        Task<DomainPerson> DomainPerson(Person database, CancellationToken cancellation);
    }
}
