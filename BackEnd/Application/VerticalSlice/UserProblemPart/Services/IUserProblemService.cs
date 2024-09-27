using Application.Shared.DTOs.Response;
using Application.VerticalSlice.UserProblemPart.DTOs.Create;
using Application.VerticalSlice.UserProblemPart.DTOs.Create.Authorized;
using Application.VerticalSlice.UserProblemPart.DTOs.Create.Unauthorized;
using System.Security.Claims;

namespace Application.VerticalSlice.UserProblemPart.Services
{
    public interface IUserProblemService
    {
        Task<ItemResponse<CreateUserProblemResponseDto>> CreateForAuthorizedAsync
            (
            IEnumerable<Claim> claims,
            CreateAuthorizedUserProblemRequestDto dto,
            CancellationToken cancellation
            );
        Task<ItemResponse<CreateUserProblemResponseDto>> CreateForUnauthorizedAsync
            (
            CreateUnauthorizedUserProblemRequestDto dto,
            CancellationToken cancellation
            );
    }
}
