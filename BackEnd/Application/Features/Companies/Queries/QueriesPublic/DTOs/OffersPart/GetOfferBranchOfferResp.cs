using Application.Shared.DTOs.Features.Companies.Responses;

namespace Application.Features.Companies.Queries.QueriesPublic.DTOs.OffersPart
{
    public class GetOfferBranchOfferResp
    {
        public CompanyResp Company { get; set; } = null!;
        public BranchResp Branch { get; set; } = null!;
        public BranchOfferResp BranchOffer { get; set; } = null!;
    }
}
