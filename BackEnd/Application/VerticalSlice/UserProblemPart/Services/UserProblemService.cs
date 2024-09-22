using Application.Shared.DTOs.Response;
using Application.Shared.Services.Authentication;
using Application.VerticalSlice.UserProblemPart.DTOs.CreateUserProblemAuthorized;
using Application.VerticalSlice.UserProblemPart.DTOs.CreateUserProblemUnauthorized;
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

        public async Task<ItemResponse<Guid>> CreateForAuthorizedAsync
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
            var id = await _userProblem.CreateUserProblemAndReturnIdAsync(problem, cancellation);
            return new ItemResponse<Guid>
            {
                Status = EnumResponseStatus.Success,
                Message = Messages.ResponseSuccess,
                Item = id,
            };
        }

        public async Task<ItemResponse<Guid>> CreateForUnauthorizedAsync
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
            var id = await _userProblem.CreateUserProblemAndReturnIdAsync(problem, cancellation);
            return new ItemResponse<Guid>
            {
                Status = EnumResponseStatus.Success,
                Message = Messages.ResponseSuccess,
                Item = id,
            };
        }
    }
}
