using Domain.Features.Branch.Entities;
using Domain.Features.Branch.ValueObjects.Identificators;
using Domain.Features.Company.Entities;
using Domain.Features.User.ValueObjects.Identificators;

namespace Application.Features.Companies.Commands.CompanyBranches.Interfaces
{
    public interface ICompanyBranchCommandRepository
    {
        //Company part
        //DML
        Task<DomainCompany> CreateCompanyAsync(DomainCompany company, CancellationToken cancellation);
        Task UpdateCompanyAsync(DomainCompany company, CancellationToken cancellation);

        //DQL
        Task<DomainCompany> GetCompanyAsync
           (
           UserId id,
           CancellationToken cancellation
           );

        //Branch Part
        //DML
        Task<IEnumerable<DomainBranch>> CreateBranchesAsync
            (
            IEnumerable<DomainBranch> branches,
            CancellationToken cancellation
            );

        Task<Dictionary<BranchId, DomainBranch>> UpdateBranchesAsync
            (
            Dictionary<BranchId, DomainBranch> branches,
            CancellationToken cancellation
            );

        //DQL
        Task<Dictionary<BranchId, DomainBranch>> GetBranchesAsync
            (
            UserId companyId,
            IEnumerable<BranchId> ids,
            CancellationToken cancellation
            );
    }
}
