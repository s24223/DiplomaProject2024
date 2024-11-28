using Application.Shared.DTOs.Features.Companies.Responses;
using Domain.Features.Company.Entities;

namespace Application.Features.Companies.Commands.CompanyBranches.DTOs.CommandsCompany.Create
{
    public class CreateCompanyResp
    {
        //Values
        public CompanyResp Company { get; set; } = null!;
        public IEnumerable<BranchResp> Branches { get; set; } = [];
        public int BranchesCount { get; private set; }



        //Cosntructor
        public CreateCompanyResp(DomainCompany domain)
        {
            Company = new CompanyResp(domain);

            Branches = domain.Branches.Select(x => new BranchResp(x.Value));
            BranchesCount = Branches.Count();
        }
    }
}
