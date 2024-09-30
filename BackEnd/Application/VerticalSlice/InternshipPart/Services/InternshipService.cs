using Application.Shared.DTOs.Response;
using Application.Shared.Services.Authentication;
using Application.VerticalSlice.InternshipPart.DTOs;
using Application.VerticalSlice.InternshipPart.Interfaces;
using Domain.Factories;
using Domain.Providers;
using System.Security.Claims;

namespace Application.VerticalSlice.InternshipPart.Services
{
    public class InternshipService : IInternshipService
    {
        private readonly IDomainFactory _domainFactory;
        private readonly IAuthenticationService _authentication;
        private readonly IDomainProvider _provider;
        private readonly IInternshipRepository _internshipRepository;

        public InternshipService(
            IDomainFactory domainFactory,
            IAuthenticationService authentication, 
            IDomainProvider provider,
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
                userId,
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
    }
}
