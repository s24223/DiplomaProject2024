using Application.Shared.DTOs.Features.Companies.Responses;

namespace Application.Features.Companies.Queries.QueriesUser.DTOs
{
    public class BranchOfferResponse
    {
        public BranchOfferResp BranchOffer { get; set; } = null!;
        public int RecruitmentDeniedCount { get; set; } = 0;
        public int RecruitmentWaitingCount { get; set; } = 0;
        public int RecruitmentAcceptedCount { get; set; } = 0;
        public int InternshipCount { get; set; } = 0;
    }
}
