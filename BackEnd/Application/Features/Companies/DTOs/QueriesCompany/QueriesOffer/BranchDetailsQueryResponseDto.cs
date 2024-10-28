using Application.Features.Companies.DTOs.QueriesCompany.Shared;
using Application.Shared.DTOs.Features.Companies;
using Domain.Features.Branch.Entities;

namespace Application.Features.Companies.DTOs.QueriesCompany.QueriesOffer
{
    public class BranchDetailsQueryResponseDto : BranchResponseDto
    {
        public CompanyDetailsQueryResponseDto Company { get; set; } = null!;

        public BranchDetailsQueryResponseDto(DomainBranch domain) : base(domain)
        {
            if (domain.Company != null)
            {
                Company = new CompanyDetailsQueryResponseDto(domain.Company);
            }
        }
    }
}
