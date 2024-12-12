using Application.Shared.DTOs.Features.Companies.Responses;
using Application.Shared.DTOs.Features.Internships;
using Domain.Features.Recruitment.Entities;

namespace Application.Features.Internships.Queries.DTOs.Recritments
{
    public class PersonRecruitmentResp
    {
        //Values
        public CompanyResp Company { get; set; } = null!;
        public BranchResp Branch { get; set; } = null!;
        public BranchOfferResp BranchOffer { get; set; } = null!;
        public OfferResp Offer { get; set; } = null!;
        public RecruitmentResp Recruitment { get; set; }
        public bool IsExistInternship { get; init; } = false;


        //Constructor
        public PersonRecruitmentResp(DomainRecruitment domain)
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
            if (domain.BranchOffer?.Branch?.Company != null)
            {
                Company = new CompanyResp(domain.BranchOffer.Branch.Company);
            }
            if (domain.Intership != null)
            {
                IsExistInternship = true;
            }
        }
    }
}
