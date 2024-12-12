using Application.Shared.DTOs.Features.Companies.Responses;
using Application.Shared.DTOs.Features.Internships;
using Application.Shared.DTOs.Features.Persons;
using Domain.Features.Recruitment.Entities;

namespace Application.Features.Internships.Queries.DTOs.Recritments
{
    public class CompanyRecruitmentResp
    {
        //Values
        public PersonPartialResp Person { get; set; } = null!;
        public BranchResp Branch { get; set; } = null!;
        public BranchOfferResp BranchOffer { get; set; } = null!;
        public OfferResp Offer { get; set; } = null!;
        public RecruitmentResp Recruitment { get; set; }
        public bool IsExistInternship { get; init; } = false;


        //Constructor
        public CompanyRecruitmentResp(DomainRecruitment domain)
        {
            Recruitment = new RecruitmentResp(domain);

            if (domain.BranchOffer != null)
            {
                BranchOffer = new BranchOfferResp(domain.BranchOffer);
            }
            if (domain.BranchOffer?.Offer != null)
            {
                Offer = new OfferResp(domain.BranchOffer.Offer);
            }
            if (domain.BranchOffer?.Branch != null)
            {
                Branch = new BranchResp(domain.BranchOffer.Branch);
            }
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
