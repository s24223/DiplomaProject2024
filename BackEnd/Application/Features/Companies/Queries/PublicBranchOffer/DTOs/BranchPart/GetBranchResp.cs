using Application.Shared.DTOs.Features.Companies.Responses;

namespace Application.Features.Companies.Queries.PublicBranchOffer.DTOs.BranchPart
{
    public class GetBranchResp
    {
        public CompanyResp Company { get; set; } = null!;
        public BranchResp Branch { get; set; } = null!;
        public IEnumerable<GetBranchBranchOfferResp> BranchOffers { get; set; } = [];
        public int TotalCount { get; set; } = 0;
    }
}
