using Application.Features.Internship.RecrutmentPart.DTOs;
using Application.Features.Internship.RecrutmentPart.DTOs.Create;
using Application.Features.Internship.RecrutmentPart.Interfaces;
using Application.Shared.DTOs.Response;
using Application.Shared.Services.Authentication;
using Domain.Shared.Factories;
using Domain.Shared.Providers;
using System.Security.Claims;

namespace Application.Features.Internship.RecrutmentPart.Services
{
    public class RecruitmentService : IRecruitmentService
    {
        private readonly IDomainFactory _domainFactory;
        private readonly IProvider _domainProvider;
        private readonly IAuthenticationService _authentication;
        private readonly IRecruitmentRepository _repository;

        public RecruitmentService(
            IDomainFactory domainFactory,
            IAuthenticationService authentication,
            IProvider domainProvider,
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
                    personId.Value,
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

        public async Task<Response> UpdateRecruitmentAsync(IEnumerable<Claim> claims, UpdateRecrutmentDto dto, CancellationToken cancellation)
        {
            var id = _authentication.GetIdNameFromClaims(claims).Value;
            var recruitment = await _repository.GetRecruitmentAsync(
                id, cancellation );
            recruitment.CompanyResponse = dto.CompanyResponse;
            recruitment.AcceptedRejected = new Domain.Shared.ValueObjects.DatabaseBool(dto.IsAccepted);
            await _repository.UpdateRecruitmentAsync(recruitment, cancellation );
            return new Response
            {
                Status = EnumResponseStatus.Success,
                Message = Messages.ResponseSuccess
            };
        }
    }
}
