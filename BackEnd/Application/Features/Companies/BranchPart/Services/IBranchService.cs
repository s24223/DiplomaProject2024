using Application.Features.Companies.BranchPart.DTOs.CreateProfile;
using Application.Features.Companies.BranchPart.DTOs.UpdateProfile;

namespace Application.Features.Companies.BranchPart.Services
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
