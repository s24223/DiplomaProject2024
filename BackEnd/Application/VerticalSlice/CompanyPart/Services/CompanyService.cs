using Application.Shared.DTOs.Response;
using Application.Shared.Services.Authentication;
using Application.VerticalSlice.CompanyPart.DTOs.CreateProfile;
using Application.VerticalSlice.CompanyPart.Interfaces;
using Domain.Factories;
using Domain.Providers;
using System.Security.Claims;

namespace Application.VerticalSlice.CompanyPart.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _repository;
        private readonly IDomainFactory _domainFactory;
        private readonly IAuthenticationService _authenticationRepository;
        private readonly IDomainProvider _domainProvider;


        public CompanyService
            (
            ICompanyRepository repository,
            IAuthenticationService authentication,
            IDomainFactory domainFactory,
            IDomainProvider domainProvider
            )
        {
            _repository = repository;
            _domainFactory = domainFactory;
            _authenticationRepository = authentication;
            _domainProvider = domainProvider;
        }

        public async Task<Response> CreateCompanyProfileAsync
            (
            IEnumerable<Claim> claims,
            CreateCompanyProfileRequestDto dto,
            CancellationToken cancellation
            )
        {
            var id = _authenticationRepository.GetIdNameFromClaims(claims);
            var domainComapany = _domainFactory.CreateDomainCompany
                (
                id.Value,
                dto.UrlSegment,
                dto.ContactEmail,
                dto.Name,
                dto.Regon,
                dto.Description,
                _domainProvider.GetTimeProvider().GetDateOnlyToday()
                );
            await _repository.CreateCompanyProfileAsync(domainComapany, cancellation);
            return new Response
            {
                Status = EnumResponseStatus.Success,
                Message = Messages.ResponseSuccess,
            };
        }
    }
}
