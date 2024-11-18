using Application.Databases.Relational.Models;
using Domain.Features.Address.Entities;
using Domain.Features.Address.ValueObjects.Identificators;
using Domain.Features.Intership.Entities;
using Domain.Features.Recruitment.Entities;

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
        DomainRecruitment ToDomainRecruitment(Recruitment databaseRecruitment);
        DomainIntership ToDomainIntership(Internship databaseIntership);
    }
}
