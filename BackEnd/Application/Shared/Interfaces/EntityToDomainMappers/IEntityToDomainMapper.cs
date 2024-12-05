using Application.Databases.Relational.Models;
using Domain.Features.Address.Entities;
using Domain.Features.Address.ValueObjects.Identificators;

namespace Application.Shared.Interfaces.EntityToDomainMappers
{
    public interface IEntityToDomainMapper
    {
        //User Module
        //Address Module
        DomainAddress ToDomainAddress
            (
            Address databaseAddress,
            Dictionary<DivisionId, AdministrativeDivision> databseDictionary
            );

        //Intership Module 
    }
}
