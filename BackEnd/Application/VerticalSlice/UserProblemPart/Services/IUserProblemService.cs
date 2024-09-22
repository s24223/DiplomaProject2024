using Application.Shared.DTOs.Response;
using Application.VerticalSlice.UserProblemPart.DTOs.CreateUserProblemAuthorized;
using Application.VerticalSlice.UserProblemPart.DTOs.CreateUserProblemUnauthorized;
using System.Security.Claims;

namespace Application.VerticalSlice.UserProblemPart.Services
{
    public interface IUserProblemService
    {
        Task<ItemResponse<Guid>> CreateForAuthorizedAsync
            (
            IEnumerable<Claim> claims,
            CreateAuthorizedUserProblemRequestDto dto,
            CancellationToken cancellation
            );
        Task<ItemResponse<Guid>> CreateForUnauthorizedAsync
            (
            CreateUnauthorizedUserProblemRequestDto dto,
            CancellationToken cancellation
            );
    }
}
