using Application.Shared.DTOs.Features.Companies.Responses;

namespace Application.Features.Companies.Queries.QueriesUser.DTOs.BranchResponse
{
    public class OfferBranchOfferResp : BranchOfferResponse
    {
        public OfferResp Offer { get; set; } = null!;
    }
}
