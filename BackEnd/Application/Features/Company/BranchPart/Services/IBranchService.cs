using Application.Features.Company.BranchPart.DTOs.CreateProfile;
using Application.Features.Company.BranchPart.DTOs.UpdateProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Company.BranchPart.Services
{
    public interface IBranchService
    {
        Task CreateBranchAsync
         (
         CreateBranchProfileRequestDto dto,
         CancellationToken cancellation
         );
        Task UpdateBranchAsync
            (
            Guid id,
            UpdateBranchProfileRequestDto dto,
            CancellationToken cancellation
            );
        
    }
}
