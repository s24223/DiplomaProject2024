using Application.Shared.DTOs.Features.Companies.Responses;
using Domain.Features.Branch.Entities;

namespace Application.Features.Companies.Queries.QueriesUser.DTOs
{
    public class GetCoreBranchesResp
    {
        //Values
        public IEnumerable<BranchResp> Branches { get; set; } = [];
        public int Count { get; set; }
        public int TotalCount { get; set; }


        //Condstructor 
        public GetCoreBranchesResp(int totalCount, IEnumerable<DomainBranch> items)
        {
            Branches = items.Select(x => new BranchResp(x));
            Count = Branches.Count();
            TotalCount = totalCount;
        }
    }
}
