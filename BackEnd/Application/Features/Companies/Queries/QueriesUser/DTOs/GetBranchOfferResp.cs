using Application.Shared.DTOs.Features.Companies.Responses;

namespace Application.Features.Companies.Queries.QueriesUser.DTOs
{
    public class GetBranchOfferResp : BranchOfferResponse
    {
        public BranchResp Branch { get; set; } = null!;
        public OfferResp Offer { get; set; } = null!;
    }
}
