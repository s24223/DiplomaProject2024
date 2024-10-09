using Application.Features.User.UserProblemPart.DTOs.Create;
using Application.Features.User.UserProblemPart.DTOs.Create.Authorized;
using Application.Features.User.UserProblemPart.DTOs.Create.Unauthorized;
using Application.Features.User.UserProblemPart.Interfaces;
using Application.Shared.DTOs.Response;
using Application.Shared.Services.Authentication;
using Domain.Features.UserProblem.ValueObjects.Identificators;
using Domain.Shared.Factories;
using Domain.Shared.Providers;
using System.Security.Claims;

namespace Application.Features.User.UserProblemPart.Services
{
    public class UserProblemService : IUserProblemService
    {
        //Values
        private readonly IProvider _provider;
        private readonly IDomainFactory _domainFactory;
        private readonly IUserProblemRepository _userProblem;
        private readonly IAuthenticationService _authentication;


        //Constructor
        public UserProblemService
            (

            IProvider provider,
            IDomainFactory domainFactory,
            IUserProblemRepository userProblem,
            IAuthenticationService authentication
            )
        {
            _provider = provider;
            _domainFactory = domainFactory;
            _userProblem = userProblem;
            _authentication = authentication;
        }


        //================================================================================================
        //================================================================================================
        //================================================================================================
        //Public Methods

        //ForAuthorized
        public async Task<ResponseItem<CreateUserProblemResponseDto>> CreateForAuthorizedAsync
            (
            IEnumerable<Claim> claims,
            CreateAuthorizedUserProblemRequestDto dto,
            CancellationToken cancellation
            )
        {
            var userId = _authentication.GetIdNameFromClaims(claims);
            var problem = _domainFactory.CreateDomainUserProblem
                (
                dto.UserMessage,
                dto.PreviousProblemId,
                null,
                userId.Value
                );

            var idUserProblem = await _userProblem.CreateAndReturnIdAsync
                (
                problem,
                cancellation
                );

            return new ResponseItem<CreateUserProblemResponseDto>
            {
                Item = new CreateUserProblemResponseDto
                {
                    IdProblem = idUserProblem,
                },
            };
        }

        public async Task<Response> AnnulForAuthorizedAsync
            (
            IEnumerable<Claim> claims,
            Guid idUserProblem,
            CancellationToken cancellation
            )
        {
            var userId = _authentication.GetIdNameFromClaims(claims);

            var domainUserProblem = await _userProblem.GetProblemAsync
                (
                userId,
                new UserProblemId(idUserProblem),
                cancellation
                );

            domainUserProblem.Annul();

            await _userProblem.SetNewStatusForAuthorizedAsync
                (
                domainUserProblem,
                cancellation
                );

            return new Response { };
        }

        //ForUnauthorized
        public async Task<ResponseItem<CreateUserProblemResponseDto>> CreateForUnauthorizedAsync
            (
            CreateUnauthorizedUserProblemRequestDto dto,
            CancellationToken cancellation
            )
        {
            var problem = _domainFactory.CreateDomainUserProblem
                (
                dto.UserMessage,
                dto.PreviousProblemId,
                dto.Email,
                null
                );

            var idUserProblem = await _userProblem.CreateAndReturnIdAsync
                (
                problem,
                cancellation
                );

            return new ResponseItem<CreateUserProblemResponseDto>
            {
                Item = new CreateUserProblemResponseDto
                {
                    IdProblem = idUserProblem,
                },
            };
        }
        //================================================================================================
        //================================================================================================
        //================================================================================================
        //Private Methods
    }
}
