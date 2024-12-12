using Application.Shared.DTOs.Features.Companies.Responses;
using Domain.Features.BranchOffer.Entities;

namespace Application.Features.Internships.Queries.DTOs.Recritments.BranchOffers
{
    public class BranchOfferRecruitmentResp
    {
        //Values
        public BranchResp Branch { get; set; } = null!;
        public OfferResp Offer { get; set; } = null!;
        public BranchOfferResp BranchOffer { get; set; } = null!;
        public IEnumerable<BranchOfferPersonRecruitmentResp>? Recruitments { get; set; } = null;
        public int Count { get; set; } = 0;
        public int TotalCount { get; set; } = 0;

        //Constructor
        public BranchOfferRecruitmentResp(DomainBranchOffer domain, int totalCount)
        {
            BranchOffer = new BranchOfferResp(domain);
            TotalCount = totalCount;

            if (domain.Offer != null)
            {
                Offer = new OfferResp(domain.Offer);
            }
            if (domain.Branch != null)
            {
                Branch = new BranchResp(domain.Branch);
            }

            if (domain.Recrutments.Any())
            {
                Recruitments = domain.Recrutments.Values
                    .Select(x => new BranchOfferPersonRecruitmentResp(x));
                Count = Recruitments.Count();
            }
        }
    }
}
