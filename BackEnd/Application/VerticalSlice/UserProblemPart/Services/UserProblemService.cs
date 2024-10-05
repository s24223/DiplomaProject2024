using Application.Shared.DTOs.Response;
using Application.Shared.Services.Authentication;
using Application.VerticalSlice.UserProblemPart.DTOs.Create;
using Application.VerticalSlice.UserProblemPart.DTOs.Create.Authorized;
using Application.VerticalSlice.UserProblemPart.DTOs.Create.Unauthorized;
using Application.VerticalSlice.UserProblemPart.Interfaces;
using Domain.Factories;
using Domain.Providers;
using Domain.ValueObjects.EntityIdentificators;
using System.Security.Claims;

namespace Application.VerticalSlice.UserProblemPart.Services
{
    public class UserProblemService : IUserProblemService
    {
        //Values
        private readonly IDomainFactory _domainFactory;
        private readonly IAuthenticationService _authenticationService;
        private readonly IProvider _domainProvider;
        private readonly IUserProblemRepository _userProblem;


        //Constructor
        public UserProblemService
            (
            IAuthenticationService authentication,
            IDomainFactory domainFactory,
            IProvider domainRepository,
            IUserProblemRepository userProblem
            )
        {
            _domainFactory = domainFactory;
            _authenticationService = authentication;
            _domainProvider = domainRepository;
            _userProblem = userProblem;
        }


        //Methods
        //================================================================================================
        //ForAuthorized
        public async Task<ResponseItem<CreateUserProblemResponseDto>> CreateForAuthorizedAsync
            (
            IEnumerable<Claim> claims,
            CreateAuthorizedUserProblemRequestDto dto,
            CancellationToken cancellation
            )
        {
            var userId = _authenticationService.GetIdNameFromClaims(claims);
            var problem = _domainFactory.CreateDomainUserProblem
                (
                dto.UserMessage,
                dto.PreviousProblemId,
                null,
                userId.Value
                );

            var idUserProblem = await _userProblem.CreateUserProblemAndReturnIdAsync
                (
                problem,
                cancellation
                );

            return new ResponseItem<CreateUserProblemResponseDto>
            {
                Status = EnumResponseStatus.Success,
                Message = Messages.ResponseSuccess,
                Item = new CreateUserProblemResponseDto
                {
                    IdProblem = idUserProblem,
                },
            };
        }

        public async Task<Response> AnnulUserProblemForAuthorizedAsync
            (
            IEnumerable<Claim> claims,
            Guid idUserProblem,
            CancellationToken cancellation
            )
        {
            var userId = _authenticationService.GetIdNameFromClaims(claims);

            var domainUserProblem = await _userProblem.GetDomainUserProblemAsync
                (
                userId,
                new UserProblemId(idUserProblem),
                cancellation
                );

            domainUserProblem.Annul();

            await _userProblem.SetNewStatusUserProblemForAuthorizedAsync
                (
                domainUserProblem,
                cancellation
                );

            return new Response
            {
                Status = EnumResponseStatus.Success,
                Message = Messages.ResponseSuccess,
            };
        }

        //================================================================================================
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

            var idUserProblem = await _userProblem.CreateUserProblemAndReturnIdAsync
                (
                problem,
                cancellation
                );

            return new ResponseItem<CreateUserProblemResponseDto>
            {
                Status = EnumResponseStatus.Success,
                Message = Messages.ResponseSuccess,
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
