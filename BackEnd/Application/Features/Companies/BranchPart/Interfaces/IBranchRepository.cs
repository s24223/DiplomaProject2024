using Domain.Features.Branch.Entities;
using Domain.Features.Branch.ValueObjects.Identificators;
using Domain.Features.User.ValueObjects.Identificators;

namespace Application.Features.Companies.BranchPart.Interfaces
{
    public interface IBranchRepository
    {
        //DML
        Task CreateAsync
            (
            DomainBranch branch,
            CancellationToken cancellation
            );

        Task UpdateAsync
            (
            DomainBranch branch,
            CancellationToken cancellation
            );

        //DQL
        Task<DomainBranch> GetBranchAsync
            (
            BranchId id,
            UserId companyId,
            CancellationToken cancellation
            );
    }
}
