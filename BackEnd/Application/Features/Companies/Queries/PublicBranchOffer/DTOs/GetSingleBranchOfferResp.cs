using Application.Shared.DTOs.Features.Companies.Responses;

namespace Application.Features.Companies.Queries.PublicBranchOffer.DTOs
{
    public class GetSingleBranchOfferResp
    {
        public CompanyResp Company { get; set; } = null!;
        public BranchResp Branch { get; set; } = null!;
        public BranchOfferResp BranchOffer { get; set; } = null!;
        public OfferResp Offer { get; set; } = null!;
        public bool? IsRecruited { get; set; } = null;
    }
}
