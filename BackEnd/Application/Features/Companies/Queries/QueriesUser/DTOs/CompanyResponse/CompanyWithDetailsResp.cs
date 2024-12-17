using Application.Shared.DTOs.Features.Characteristics.Responses;
using Application.Shared.DTOs.Features.Companies.Responses;

namespace Application.Features.Companies.Queries.QueriesUser.DTOs.CompanyResponse
{
    public class CompanyWithDetailsResp
    {
        public CompanyResp Company { get; set; } = null!;
        public int BranchesTotalCount { get; set; } = 0;
        public int OffersTotalCount { get; set; } = 0;
        public int BranchOfferPastCount { get; set; } = 0;
        public int BranchOfferActiveCount { get; set; } = 0;
        public int BranchOfferFutureCount { get; set; } = 0;
        public int RecruitmentDeniedCount { get; set; } = 0;
        public int RecruitmentWaitingCount { get; set; } = 0;
        public int RecruitmentAcceptedCount { get; set; } = 0;
        public int InternshipCount { get; set; } = 0;
        public IEnumerable<CharAndCharTypeResp>? CompanyCharacteristics { get; set; } = null;
        private IEnumerable<BranchWithDeatilsToCompanyResp> _branches { get; set; } = [];
        public IEnumerable<BranchWithDeatilsToCompanyResp> Branches
        {
            get { return _branches; }
            set
            {
                BranchesCount = value.Count();
                _branches = value;
            }
        }
        public int BranchesCount { get; private set; } = 0;
        private IEnumerable<OfferWithDetailsToCompanyResp> _offers { get; set; } = [];
        public IEnumerable<OfferWithDetailsToCompanyResp> Offers
        {
            get { return _offers; }
            set
            {
                OffersCount = value.Count();
                _offers = value;
            }
        }
        public int OffersCount { get; private set; } = 0;
    }
}
