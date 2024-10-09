using Domain.Features.Branch.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Companies.BranchPart.Interfaces
{
    public interface IBranchRepository
    {
        Task CreateBranchProfileAsync
            (
            DomainBranch branch,
            CancellationToken cancellation
            );
        Task UpdateBranchProfileAsync
            (

            DomainBranch branch,
            CancellationToken cancellation
            );
    }
}
