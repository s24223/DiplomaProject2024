using Domain.Features.Branch.Entities;
using Domain.Features.Branch.ValueObjects.Identificators;
using Domain.Features.User.ValueObjects.Identificators;

namespace Application.Features.Companies.Interfaces.CommandsCompanyBranch
{
    public interface IBranchRepository
    {
        //DML
        Task<Guid> CreateAsync
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
