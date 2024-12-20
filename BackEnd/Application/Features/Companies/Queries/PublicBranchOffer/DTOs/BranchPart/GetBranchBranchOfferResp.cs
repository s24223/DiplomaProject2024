using Application.Shared.DTOs.Features.Companies.Responses;

namespace Application.Features.Companies.Queries.PublicBranchOffer.DTOs.BranchPart
{
    public class GetBranchBranchOfferResp
    {
        public BranchOfferResp BranchOffer { get; set; } = null!;
        public OfferResp Offer { get; set; } = null!;
    }
}
