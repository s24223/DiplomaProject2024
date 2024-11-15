using Application.Shared.DTOs.Features.Companies.Responses;
using Domain.Features.Company.Entities;

namespace Application.Features.Companies.DTOs.CommandsCompanyBranch.CommandsCompany.Create
{
    public class CreateCompanyResponseDto
    {
        //Values
        public CompanyResponseDto Company { get; set; } = null!;
        public IEnumerable<BranchResponseDto> Branches { get; set; } = [];
        public int BranchesCount { get; private set; }



        //Cosntructor
        public CreateCompanyResponseDto(DomainCompany domain)
        {
            Company = new CompanyResponseDto(domain);

            Branches = domain.Branches.Select(x => new BranchResponseDto(x.Value));
            BranchesCount = Branches.Count();
        }
    }
}
