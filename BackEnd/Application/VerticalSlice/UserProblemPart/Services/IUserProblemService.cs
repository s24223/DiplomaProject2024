using Application.Shared.DTOs.Response;
using Application.VerticalSlice.UserProblemPart.DTOs.Create;
using Application.VerticalSlice.UserProblemPart.DTOs.Create.Authorized;
using Application.VerticalSlice.UserProblemPart.DTOs.Create.Unauthorized;
using System.Security.Claims;

namespace Application.VerticalSlice.UserProblemPart.Services
{
    public interface IUserProblemService
    {
        //================================================================================================
        //ForAuthorized
        Task<ItemResponse<CreateUserProblemResponseDto>> CreateForAuthorizedAsync
            (
            IEnumerable<Claim> claims,
            CreateAuthorizedUserProblemRequestDto dto,
            CancellationToken cancellation
            );
        Task<Response> AnnulUserProblemForAuthorizedAsync
            (
            IEnumerable<Claim> claims,
            Guid idUserProblem,
            CancellationToken cancellation
            );

        //================================================================================================
        //ForUnauthorized
        Task<ItemResponse<CreateUserProblemResponseDto>> CreateForUnauthorizedAsync
            (
            CreateUnauthorizedUserProblemRequestDto dto,
            CancellationToken cancellation
            );
    }
}
