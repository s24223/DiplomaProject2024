using Application.Databases.Relational.Models;
using Domain.Features.Intership.Entities;
using Domain.Features.Recruitment.Entities;
using Domain.Shared.Factories;

namespace Application.Features.Internships.Mappers
{
    public class InternshipMapper : IInternshipMapper
    {
        //Values
        private readonly IDomainFactory _domainFactory;


        //Cosnructor
        public InternshipMapper(IDomainFactory domainFactory)
        {
            _domainFactory = domainFactory;
        }


        //==============================================================================================================
        //==============================================================================================================
        //==============================================================================================================
        //Intership Module
        public DomainRecruitment DomainRecruitment(Recruitment database)
        {
            return _domainFactory.CreateDomainRecruitment
                (
                database.Id,
                database.PersonId,
                database.BranchOfferId,
                database.Created,
                database.PersonMessage,
                database.CompanyResponse,
                database.IsAccepted
                );
        }

        public DomainIntership DomainIntership(Internship database)
        {
            return _domainFactory.CreateDomainIntership
                (
                database.Id,
                database.Created,
                database.ContractStartDate,
                database.ContractEndDate,
                database.ContractNumber
                );
        }
    }
}
