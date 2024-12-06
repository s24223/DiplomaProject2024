using Application.Databases.Relational.Models;
using Domain.Features.Address.Entities;
using Domain.Features.Address.ValueObjects.Identificators;

namespace Application.Features.Addresses.Mappers
{
    public interface IAddressMapper
    {
        DomainAddress DomainAddress
            (
            Address databaseAddress,
            Dictionary<DivisionId, AdministrativeDivision> databseDictionary
            );
    }
}
