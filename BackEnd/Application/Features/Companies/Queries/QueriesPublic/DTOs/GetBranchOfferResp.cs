using Application.Shared.DTOs.Features.Companies.Responses;

namespace Application.Features.Companies.Queries.QueriesPublic.DTOs
{
    public class GetBranchOfferResp
    {
        public CompanyResp Company { get; set; } = null!;
        public BranchResp Branch { get; set; } = null!;
        public BranchOfferResp BranchOffer { get; set; } = null!;
        public OfferResp Offer { get; set; } = null!;
    }
}
