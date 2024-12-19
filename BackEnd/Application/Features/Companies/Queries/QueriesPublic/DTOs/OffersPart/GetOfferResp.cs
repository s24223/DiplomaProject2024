using Application.Shared.DTOs.Features.Companies.Responses;

namespace Application.Features.Companies.Queries.QueriesPublic.DTOs.OffersPart
{
    public class GetOfferResp
    {
        public OfferResp Offer { get; set; } = null!;
        public IEnumerable<GetOfferBranchOfferResp> BranchOffers { get; set; } = [];
        public int TotalCount { get; set; } = 0;
    }
}
