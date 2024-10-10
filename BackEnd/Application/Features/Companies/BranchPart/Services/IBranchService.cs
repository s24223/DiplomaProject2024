using Application.Features.Companies.BranchPart.DTOs.Create;
using Application.Features.Companies.BranchPart.DTOs.Update;
using Application.Shared.DTOs.Response;
using System.Security.Claims;

namespace Application.Features.Companies.BranchPart.Services
{
    public interface IBranchService
    {
        //DML
        Task<ResponseItem<CreateBranchResponseDto>> CreateAsync
         (
         IEnumerable<Claim> claims,
         CreateBranchRequestDto dto,
         CancellationToken cancellation
         );

        Task<Response> UpdateAsync
            (
            IEnumerable<Claim> claims,
            Guid branchId,
            UpdateBranchRequestDto dto,
            CancellationToken cancellation
            );
        //DQL
    }
}
