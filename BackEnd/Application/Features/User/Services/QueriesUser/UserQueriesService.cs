using Application.Features.User.DTOs.QueriesUser.FullUserData;
using Application.Features.User.Interfaces.QueriesUser;
using Application.Shared.DTOs.Response;
using Application.Shared.Services.Authentication;
using System.Security.Claims;

namespace Application.Features.User.Services.QueriesUser
{
    public class UserQueriesService : IUserQueriesService
    {
        //Values
        private readonly IUserQueriesRepository _queriesRepository;
        private readonly IAuthenticationService _authenticationService;

        //Controller
        public UserQueriesService
            (
            IUserQueriesRepository queriesRepository,
            IAuthenticationService authenticationService
            )
        {
            _queriesRepository = queriesRepository;
            _authenticationService = authenticationService;
        }


        //================================================================================================
        //================================================================================================
        //================================================================================================
        //Public Methods
        public async Task<ResponseItem<UserQueryResponseDto>> GetPersonalDataAsync
            (
            IEnumerable<Claim> claims,
            CancellationToken cancellation
            )
        {
            var id = _authenticationService.GetIdNameFromClaims(claims);
            var domainUser = await _queriesRepository.GetUserDataAsync(id, cancellation);
            return new ResponseItem<UserQueryResponseDto>
            {
                Item = new UserQueryResponseDto(domainUser),
            };
        }
        //================================================================================================
        //================================================================================================
        //================================================================================================
        //Private Methods
    }
}
