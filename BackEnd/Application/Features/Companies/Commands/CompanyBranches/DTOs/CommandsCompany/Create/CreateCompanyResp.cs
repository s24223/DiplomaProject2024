using Application.Features.Companies.Commands.CompanyBranches.DTOs.CommandsBranch.Create;
using Application.Shared.DTOs.Features.Companies.Responses;
using Domain.Features.Branch.Entities;
using Domain.Features.Company.Entities;

namespace Application.Features.Companies.Commands.CompanyBranches.DTOs.CommandsCompany.Create
{
    public class CreateCompanyResp
    {
        //Values
        public CompanyResp? Company { get; set; } = null!;
        public IEnumerable<CreateBranchesResp> Branches { get; set; } = [];



        //Cosntructor
        public CreateCompanyResp
            (
            DomainCompany company,
            IEnumerable<(DomainBranch Item, bool IsDuplicate)> items,
            bool isBeforeDb,
            bool hasDuplicates
            )
        {
            Company = new CompanyResp(company);
            Branches = items.Select(x =>
                new CreateBranchesResp(x.Item, x.IsDuplicate, isBeforeDb, hasDuplicates));
        }

        public CreateCompanyResp(DomainCompany company)
        {
            Company = new CompanyResp(company);
            Branches = company.Branches.Select(x =>
                new CreateBranchesResp(x.Value, false, false, false));
        }
    }
}
