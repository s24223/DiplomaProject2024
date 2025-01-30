using Application.Databases.Relational.Models;
using Domain.Features.Comment.Entities;
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
            var domain = _domainFactory.CreateDomainRecruitment
                (
                database.Id,
                database.PersonId,
                database.BranchOfferId,
                database.Created,
                database.PersonMessage,
                database.CompanyResponse,
                database.IsAccepted
                );
            domain.Url = database.CvUrl;
            return domain;
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

        public DomainComment DomainComment(Comment database)
        {
            return _domainFactory.CreateDomainComment(
                database.InternshipId,
                database.CommentTypeId,
                database.Created,
                database.Description,
                database.Evaluation
                );
        }
    }
}
