using Application.Shared.DTOs.Response;
using Application.Shared.Services.Authentication;
using Application.VerticalSlice.UserProblemPart.DTOs.Create;
using Application.VerticalSlice.UserProblemPart.DTOs.Create.Authorized;
using Application.VerticalSlice.UserProblemPart.DTOs.Create.Unauthorized;
using Application.VerticalSlice.UserProblemPart.Interfaces;
using Domain.Factories;
using Domain.Providers;
using System.Security.Claims;

namespace Application.VerticalSlice.UserProblemPart.Services
{
    public class UserProblemService : IUserProblemService
    {
        private readonly IDomainFactory _domainFactory;
        private readonly IAuthenticationService _authenticationService;
        private readonly IDomainProvider _domainProvider;
        private readonly IUserProblemRepository _userProblem;


        public UserProblemService
            (
            IAuthenticationService authentication,
            IDomainFactory domainFactory,
            IDomainProvider domainRepository,
            IUserProblemRepository userProblem
            )
        {
            _domainFactory = domainFactory;
            _authenticationService = authentication;
            _domainProvider = domainRepository;
            _userProblem = userProblem;
        }

        public async Task<ItemResponse<CreateUserProblemResponseDto>> CreateForAuthorizedAsync
            (
            IEnumerable<Claim> claims,
            CreateAuthorizedUserProblemRequestDto dto,
            CancellationToken cancellation
            )
        {
            var userId = _authenticationService.GetIdNameFromClaims(claims);
            var problem = _domainFactory.CreateDomainUserProblem
                (
                null,
                null,
                dto.UserMessage,
                null,
                dto.PreviousProblemId,
                null,
                null,
                userId
                );
            var idUserProblem = await _userProblem.CreateUserProblemAndReturnIdAsync
                (
                problem,
                cancellation
                );
            return new ItemResponse<CreateUserProblemResponseDto>
            {
                Status = EnumResponseStatus.Success,
                Message = Messages.ResponseSuccess,
                Item = new CreateUserProblemResponseDto
                {
                    IdUserProblem = idUserProblem,
                },
            };
        }

        public async Task<ItemResponse<CreateUserProblemResponseDto>> CreateForUnauthorizedAsync
            (
            CreateUnauthorizedUserProblemRequestDto dto,
            CancellationToken cancellation
            )
        {
            var problem = _domainFactory.CreateDomainUserProblem
                (
                null,
                null,
                dto.UserMessage,
                null,
                dto.PreviousProblemId,
                dto.Email,
                null,
                null
                );
            var idUserProblem = await _userProblem.CreateUserProblemAndReturnIdAsync
                (
                problem,
                cancellation
                );
            return new ItemResponse<CreateUserProblemResponseDto>
            {
                Status = EnumResponseStatus.Success,
                Message = Messages.ResponseSuccess,
                Item = new CreateUserProblemResponseDto
                {
                    IdUserProblem = idUserProblem,
                },
            };
        }
    }
}
