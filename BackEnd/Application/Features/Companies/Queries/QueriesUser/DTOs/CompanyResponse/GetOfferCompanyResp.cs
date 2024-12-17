using Application.Shared.DTOs.Features.Companies.Responses;

namespace Application.Features.Companies.Queries.QueriesUser.DTOs.CompanyResponse
{
    public class GetOfferCompanyResp
    {
        public OfferResp Offer { get; set; } = null!;
        public int BranchOfferPastCount { get; set; } = 0;
        public int BranchOfferActiveCount { get; set; } = 0;
        public int BranchOfferFutureCount { get; set; } = 0;
        public int RecruitmentDeniedCount { get; set; } = 0;
        public int RecruitmentWaitingCount { get; set; } = 0;
        public int RecruitmentAcceptedCount { get; set; } = 0;
        public int InternshipCount { get; set; } = 0;
    }
}
