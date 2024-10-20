using Application.Databases.Relational.Models;
using Domain.Features.Address.Entities;
using Domain.Features.Branch.Entities;
using Domain.Features.BranchOffer.Entities;
using Domain.Features.Company.Entities;
using Domain.Features.Intership.Entities;
using Domain.Features.Offer.Entities;
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

        //Address Module
        DomainAddress ToDomainAddress
            (
            Address databaseAddress,
            IEnumerable<AdministrativeDivision> databseHierarchy
            );

        //Company Module
        DomainCompany ToDomainCompany(Company databaseCompany);
        DomainBranch ToDomainBranch(Branch databaseBranch);
        DomainOffer ToDomainOffer(Offer databaseOffer);
        DomainBranchOffer ToDomainBranchOffer(BranchOffer databaseBranchOffer);

        //Person Module
        DomainPerson ToDomainPerson(Person databasePerson);

        //Intership Module 
        DomainRecruitment ToDomainRecruitment(Recruitment databaseRecruitment);
        DomainIntership ToDomainIntership(Internship databaseIntership);
    }
}
