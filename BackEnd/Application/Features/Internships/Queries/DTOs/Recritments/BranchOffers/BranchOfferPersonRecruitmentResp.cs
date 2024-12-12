using Application.Shared.DTOs.Features.Internships;
using Application.Shared.DTOs.Features.Persons;
using Domain.Features.Recruitment.Entities;

namespace Application.Features.Internships.Queries.DTOs.Recritments.BranchOffers
{
    public class BranchOfferPersonRecruitmentResp
    {
        //values
        public PersonPartialResp Person { get; set; } = null!;
        public RecruitmentResp Recruitment { get; set; }
        public bool IsExistInternship { get; init; } = false;


        //Constructor
        public BranchOfferPersonRecruitmentResp(DomainRecruitment domain)
        {
            Recruitment = new RecruitmentResp(domain);

            if (domain.Person != null)
            {
                Person = new PersonPartialResp(domain.Person);
            }
            if (domain.Intership != null)
            {
                IsExistInternship = true;
            }
        }
    }
}
