using Application.Shared.DTOs.Features.Companies.Responses;

namespace Application.Features.Companies.Queries.PublicCompany.DTOs
{
    public class GetOfferQueryResp
    {
        public OfferResp Offer { get; set; } = null!;
        public int ActiveBranchOfferCount { get; set; } = 0;
    }
}
