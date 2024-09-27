using Application.Shared.DTOs.Response;
using Application.Shared.Services.Authentication;
using Application.VerticalSlice.RecrutmentPart.DTOs.Create;
using Application.VerticalSlice.RecrutmentPart.Interfaces;
using Domain.Factories;
using Domain.Providers;
using System.Security.Claims;

namespace Application.VerticalSlice.RecrutmentPart.Services
{
    public class RecruitmentService : IRecruitmentService
    {
        private readonly IDomainFactory _domainFactory;
        private readonly IDomainProvider _domainProvider;
        private readonly IAuthenticationService _authentication;
        private readonly IRecruitmentRepository _repository;

        public RecruitmentService(
            IDomainFactory domainFactory,
            IAuthenticationService authentication,
            IDomainProvider domainProvider,
            IRecruitmentRepository repository
            )
        {
            _domainFactory = domainFactory;
            _authentication = authentication;
            _domainProvider = domainProvider;
            _repository = repository;
        }


        public async Task<Response> CreateAsync
            (
            IEnumerable<Claim> claims,
            CreateRecruitmentRequestDto dto,
            CancellationToken cancellation
            )
        {
            var personId = _authentication.GetIdNameFromClaims(claims);
            var recruitment = _domainFactory.CreateDomainRecruitment(
                    personId,
                    dto.BranchId,
                    dto.OfferId,
                    dto.Created,
                    null,
                    dto.PersonMessage,
                    null,
                    null
                );
            await _repository.CreateAsync(recruitment, cancellation);
            return new Response
            {
                Status = EnumResponseStatus.Success,
                Message = Messages.ResponseSuccess
            };
        }

    }
}
