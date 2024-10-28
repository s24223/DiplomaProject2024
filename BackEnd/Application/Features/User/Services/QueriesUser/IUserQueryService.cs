using Application.Features.User.DTOs.QueriesUser.FullUserData;
using Application.Shared.DTOs.Response;
using System.Security.Claims;

namespace Application.Features.User.Services.QueriesUser
{
    public interface IUserQueryService
    {
        Task<ResponseItem<UserQueryResponseDto>> GetPersonalDataAsync
            (
            IEnumerable<Claim> claims,
            CancellationToken cancellation
            );
    }
}
