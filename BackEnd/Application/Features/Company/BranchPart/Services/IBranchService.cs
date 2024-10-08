using Application.Features.Company.BranchPart.DTOs.CreateProfile;
using Application.Features.Company.BranchPart.DTOs.UpdateProfile;

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
