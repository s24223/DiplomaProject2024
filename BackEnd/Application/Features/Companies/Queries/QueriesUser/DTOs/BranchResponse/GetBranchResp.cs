using Application.Shared.DTOs.Features.Companies.Responses;

namespace Application.Features.Companies.Queries.QueriesUser.DTOs.BranchResponse
{
    public class GetBranchResp
    {
        public BranchResp Branch { get; set; } = null!;
        public int BranchOfferPastCount { get; set; } = 0;
        public int BranchOfferActiveCount { get; set; } = 0;
        public int BranchOfferFutureCount { get; set; } = 0;
        public int RecruitmentDeniedCount { get; set; } = 0;
        public int RecruitmentWaitingCount { get; set; } = 0;
        public int RecruitmentAcceptedCount { get; set; } = 0;
        public int InternshipCount { get; set; } = 0;
        private IEnumerable<OfferBranchOfferResp> _branchOffers { get; set; } = [];
        public IEnumerable<OfferBranchOfferResp> BranchOffers
        {
            get { return _branchOffers; }
            set
            {
                Count = value.Count();
                _branchOffers = value;
            }
        }
        public int Count { get; set; } = 0;
    }
}
