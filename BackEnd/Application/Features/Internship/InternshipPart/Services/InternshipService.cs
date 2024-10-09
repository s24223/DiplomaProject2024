using Application.Features.Internship.InternshipPart.DTOs;
using Application.Features.Internship.InternshipPart.Interfaces;
using Application.Shared.DTOs.Response;
using Application.Shared.Services.Authentication;
using Domain.Shared.Factories;
using Domain.Shared.Providers;
using System.Security.Claims;

namespace Application.Features.Internship.InternshipPart.Services
{
    public class InternshipService : IInternshipService
    {
        private readonly IDomainFactory _domainFactory;
        private readonly IAuthenticationService _authentication;
        private readonly IProvider _provider;
        private readonly IInternshipRepository _internshipRepository;

        public InternshipService(
            IDomainFactory domainFactory,
            IAuthenticationService authentication,
            IProvider provider,
            IInternshipRepository internshipRepository)
        {
            _domainFactory = domainFactory;
            _authentication = authentication;
            _provider = provider;
            _internshipRepository = internshipRepository;
        }

        public async Task<Response> CreateInternshipAsync(IEnumerable<Claim> claims, CreateInternshipDto dto, CancellationToken cancellation)
        {
            var userId = _authentication.GetIdNameFromClaims(claims);
            var internship = _domainFactory.CreateDomainInternship(
                dto.ContactNumber,
                userId.Value,
                dto.BranchId,
                dto.OfferId,
                dto.Created
                );
            await _internshipRepository.CreateInternshipAsync(
                internship, cancellation);
            return new Response
            {
                Status = EnumResponseStatus.Success,
                Message = Messages.ResponseSuccess
            };
        }

        public async Task<Response> UpdateInternshipAsync(Guid id, UpdateInternshipDto dto, CancellationToken cancellation)
        {
            var internship = await _internshipRepository
                .GetInternshipAsync(id, cancellation);
            internship.ContractNumber = dto.ContractNumber;
            await _internshipRepository.UpdateInternshipAsync(
                internship, cancellation);
            return new Response
            {
                Status = EnumResponseStatus.Success,
                Message = Messages.ResponseSuccess
            };
        }
    }
}
