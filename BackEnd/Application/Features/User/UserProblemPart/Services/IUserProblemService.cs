using Application.Features.User.UserProblemPart.DTOs.Create;
using Application.Features.User.UserProblemPart.DTOs.Create.Authorized;
using Application.Features.User.UserProblemPart.DTOs.Create.Unauthorized;
using Application.Shared.DTOs.Response;
using System.Security.Claims;

namespace Application.Features.User.UserProblemPart.Services
{
    public interface IUserProblemService
    {
        //================================================================================================
        //ForAuthorized
        Task<ResponseItem<CreateUserProblemResponseDto>> CreateForAuthorizedAsync
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
        Task<ResponseItem<CreateUserProblemResponseDto>> CreateForUnauthorizedAsync
            (
            CreateUnauthorizedUserProblemRequestDto dto,
            CancellationToken cancellation
            );
    }
}
