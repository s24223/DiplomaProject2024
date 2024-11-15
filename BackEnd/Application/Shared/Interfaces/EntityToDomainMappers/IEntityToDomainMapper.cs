using Application.Databases.Relational.Models;
using Domain.Features.Address.Entities;
using Domain.Features.Address.ValueObjects.Identificators;
using Domain.Features.Intership.Entities;
using Domain.Features.Notification.Entities;
using Domain.Features.Person.Entities;
using Domain.Features.Recruitment.Entities;
using Domain.Features.Url.Entities;
using Domain.Features.User.Entities;

namespace Application.Shared.Interfaces.EntityToDomainMappers
{
    public interface IEntityToDomainMapper
    {
        //User Module
        DomainUser ToDomainUser(User databaseUser);
        DomainUrl ToDomainUrl(Url databaseUrl);
        DomainNotification ToDomainNotification(Notification notification);

        //Address Module
        DomainAddress ToDomainAddress
            (
            Address databaseAddress,
            Dictionary<DivisionId, AdministrativeDivision> databseDictionary
            );



        //Person Module
        DomainPerson ToDomainPerson(Person databasePerson);

        //Intership Module 
        DomainRecruitment ToDomainRecruitment(Recruitment databaseRecruitment);
        DomainIntership ToDomainIntership(Internship databaseIntership);
    }
}
