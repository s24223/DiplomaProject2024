using Application.Shared.DTOs.Features.Characteristics.Responses;
using Application.Shared.DTOs.Features.Companies.Responses;

namespace Application.Features.Companies.Queries.PublicCompany.DTOs
{
    public class GetBranchQueryResp
    {
        public BranchResp Branch { get; set; } = null!;
        public IEnumerable<CharResp> Characteristics { get; set; } = [];
        public int BranchOffersCount { get; set; } = 0;
    }
}
