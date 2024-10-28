using Application.Shared.DTOs.Features.Companies;
using Domain.Features.Company.Entities;

namespace Application.Features.Companies.DTOs.CommandsCompanyBranch.CommandsCompany.Create
{
    public class CreateCompanyResponseDto : CompanyResponseDto
    {
        public IEnumerable<BranchResponseDto> Branches { get; set; } = [];
        public CreateCompanyResponseDto(DomainCompany domain) : base(domain)
        {
            Branches = domain.Branches.Select(x => new BranchResponseDto(x.Value));
        }
    }
}
