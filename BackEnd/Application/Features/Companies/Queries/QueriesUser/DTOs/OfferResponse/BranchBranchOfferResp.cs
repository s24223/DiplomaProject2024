using Application.Shared.DTOs.Features.Companies.Responses;

namespace Application.Features.Companies.Queries.QueriesUser.DTOs.OfferResponse
{
    public class BranchBranchOfferResp : BranchOfferResponse
    {
        public BranchResp Branch { get; set; } = null!;
    }
}
