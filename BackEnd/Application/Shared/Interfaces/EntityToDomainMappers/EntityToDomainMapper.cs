using Application.Database.Models;
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
using Domain.Features.UserProblem.Entities;
using Domain.Shared.Factories;

namespace Application.Shared.Interfaces.EntityToDomainMappers
{
    public class EntityToDomainMapper : IEntityToDomainMapper
    {
        //Values
        private readonly IDomainFactory _domainFactory;


        //Cosnructor
        public EntityToDomainMapper
            (
            IDomainFactory domainFactory
            )
        {
            _domainFactory = domainFactory;
        }


        //================================================================================================================
        //================================================================================================================
        //================================================================================================================
        //Public Methods
        //User Module
        public DomainUser ToDomainUser(User databaseUser)
        {
            return _domainFactory.CreateDomainUser
                (
                databaseUser.Id,
                databaseUser.Login,
                databaseUser.LastLoginIn,
                databaseUser.LastPasswordUpdate
                );
        }

        public DomainUserProblem ToDomainUserProblem(UserProblem databaseUserProblem)
        {
            return _domainFactory.CreateDomainUserProblem
                (
                databaseUserProblem.Id,
                databaseUserProblem.Created,
                databaseUserProblem.UserMessage,
                databaseUserProblem.Response,
                databaseUserProblem.PreviousProblemId,
                databaseUserProblem.Email,
                databaseUserProblem.Status,
                databaseUserProblem.UserId
                );
        }

        public DomainUrl ToDomainUrl(Url databaseUrl)
        {
            return _domainFactory.CreateDomainUrl
                (
                databaseUrl.UserId,
                databaseUrl.UrlTypeId,
                databaseUrl.Created,
                databaseUrl.Path,
                databaseUrl.Name,
                databaseUrl.Description
                );
        }


        //Address Module
        public DomainAddress ToDomainAddress
            (
            Address databaseAddress,
            IEnumerable<AdministrativeDivision> databseHierarchy
            )
        {
            var domainHierachy = databseHierarchy.Select(x => new DomainAdministrativeDivision
                (
                x.Id,
                x.Name,
                x.ParentDivisionId,
                x.AdministrativeType.Id,
                x.AdministrativeType.Name
                )).ToList();


            var address = _domainFactory.CreateDomainAddress
                (
                databaseAddress.Id,
                databaseAddress.DivisionId,
                databaseAddress.StreetId,
                databaseAddress.BuildingNumber,
                databaseAddress.ApartmentNumber,
                databaseAddress.ZipCode
                );
            address.Street = new DomainStreet
                (
                databaseAddress.Street.Id,
                databaseAddress.Street.Name,
                (databaseAddress.Street.AdministrativeType == null
                ? null : databaseAddress.Street.AdministrativeType.Id),
                (databaseAddress.Street.AdministrativeType == null
                ? null : databaseAddress.Street.AdministrativeType.Name)
                );
            address.SetHierarchy(domainHierachy);
            return address;
        }


        //Company Module
        public DomainCompany ToDomainCompany(Company databaseCompany)
        {
            return _domainFactory.CreateDomainCompany
                (
                databaseCompany.UserId,
                databaseCompany.UrlSegment,
                databaseCompany.ContactEmail,
                databaseCompany.Name,
                databaseCompany.Regon,
                databaseCompany.Description
                );
        }

        public DomainBranch ToDomainBranch(Branch databaseBranch)
        {
            return _domainFactory.CreateDomainBranch
                (
                databaseBranch.Id,
                databaseBranch.CompanyId,
                databaseBranch.AddressId,
                databaseBranch.UrlSegment,
                databaseBranch.Name,
                databaseBranch.Description
                );
        }

        public DomainOffer ToDomainOffer(Offer databaseOffer)
        {
            return _domainFactory.CreateDomainOffer
                (
                databaseOffer.Id,
                databaseOffer.Name,
                databaseOffer.Description,
                databaseOffer.MinSalary,
                databaseOffer.MaxSalary,
                databaseOffer.IsNegotiatedSalary,
                databaseOffer.IsForStudents
                );
        }

        public DomainBranchOffer ToDomainBranchOffer(BranchOffer databaseBranchOffer)
        {
            return _domainFactory.CreateDomainBranchOffer
                (
                databaseBranchOffer.BranchId,
                databaseBranchOffer.OfferId,
                databaseBranchOffer.Created,
                databaseBranchOffer.PublishStart,
                databaseBranchOffer.PublishEnd,
                databaseBranchOffer.WorkStart,
                databaseBranchOffer.WorkEnd,
                databaseBranchOffer.LastUpdate
                );
        }


        //Person Module
        public DomainPerson ToDomainPerson(Person databasePerson)
        {
            return _domainFactory.CreateDomainPerson
                (
                databasePerson.UserId,
                databasePerson.UrlSegment,
                databasePerson.Created,
                databasePerson.ContactEmail,
                databasePerson.Name,
                databasePerson.Surname,
                databasePerson.BirthDate,
                databasePerson.ContactPhoneNum,
                databasePerson.Description,
                databasePerson.IsStudent,
                databasePerson.IsPublicProfile,
                databasePerson.AddressId
                );
        }

        //Intership Module
        public DomainRecruitment ToDomainRecruitment(Recruitment databaseRecruitment)
        {
            return _domainFactory.CreateDomainRecruitment
                (
                databaseRecruitment.PersonId,
                databaseRecruitment.BranchId,
                databaseRecruitment.OfferId,
                databaseRecruitment.Created,
                databaseRecruitment.ApplicationDate,
                databaseRecruitment.PersonMessage,
                databaseRecruitment.CompanyResponse,
                databaseRecruitment.IsAccepted
                );
        }

        public DomainIntership ToDomainIntership(Internship databaseIntership)
        {
            return _domainFactory.CreateDomainIntership
                (
                databaseIntership.Id,
                databaseIntership.PersonId,
                databaseIntership.BranchId,
                databaseIntership.OfferId,
                databaseIntership.Created,
                databaseIntership.ContractNumber
                );
        }

        //================================================================================================================
        //================================================================================================================
        //================================================================================================================
        //Private Methods
    }
}
