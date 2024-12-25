using Application.Shared.DTOs.Features.Characteristics.Responses;
using Application.Shared.DTOs.Features.Companies.Responses;

namespace Application.Features.Companies.Queries.PublicCompany.DTOs
{
    public class GetCompanyItemResp
    {
        public CompanyResp Company { get; set; } = null!;
        public int TotalBranches { get; set; } = 0;
        public int TotalOffers { get; set; } = 0;
        public int TotalActiveBranchOffers { get; set; } = 0;
        public IEnumerable<CharResp> Characteristics { get; set; } = [];
    }
}
