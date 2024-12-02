using Domain.Features.Branch.Entities;
using Domain.Features.Branch.ValueObjects.Identificators;
using Domain.Features.Company.Entities;
using Domain.Features.User.ValueObjects.Identificators;

namespace Application.Features.Companies.Commands.CompanyBranches.Interfaces
{
    public interface ICompanyBranchCmdRepo
    {
        Task<string?> CheckDbDuplicatesCompanyAsync
            (
            DomainCompany domain,
            bool isCreating,
            CancellationToken cancellation
            );

        Task<(IEnumerable<(DomainBranch Item, bool IsDuplicate)> Items, bool HasDuplicates)>
            CheckDbDuplicatesBranchesCreateAsync
            (
            IEnumerable<DomainBranch> domains,
            CancellationToken cancellation
            );

        Task<(IEnumerable<(DomainBranch Item, bool IsDuplicate)> Items, bool HasDuplicates)>
            CheckDbDuplicatesBranchesUpdateAsync
            (
            Dictionary<BranchId, DomainBranch> dictionary,
            CancellationToken cancellation
            );

        //Company part
        //DML
        Task<DomainCompany> CreateCompanyAsync
            (
            DomainCompany company,
            CancellationToken cancellation
            );

        Task<DomainCompany> UpdateCompanyAsync
            (
            DomainCompany company,
            CancellationToken cancellation
            );

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

        Task<IEnumerable<DomainBranch>> UpdateBranchesAsync
            (
            Dictionary<BranchId, DomainBranch> branches,
            CancellationToken cancellation
            );

        //DQL
        Task<Dictionary<BranchId, DomainBranch>> GetBranchesAsync
            (UserId companyId, IEnumerable<BranchId> ids, CancellationToken cancellation);
    }
}
