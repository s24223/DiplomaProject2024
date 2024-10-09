using Application.Features.Companies.CompanyPart.DTOs.Create;
using Application.Features.Companies.CompanyPart.DTOs.Update;
using Application.Features.Companies.CompanyPart.Interfaces;
using Application.Shared.DTOs.Response;
using Application.Shared.Services.Authentication;
using Domain.Shared.Factories;
using Domain.Shared.Providers;
using System.Security.Claims;

namespace Application.Features.Companies.CompanyPart.Services
{
    public class CompanyService : ICompanyService
    {
        //Values
        private readonly ICompanyRepository _repository;
        private readonly IDomainFactory _domainFactory;
        private readonly IAuthenticationService _authenticationRepository;
        private readonly IProvider _domainProvider;


        //Cosntructor
        public CompanyService
            (
            ICompanyRepository repository,
            IAuthenticationService authentication,
            IDomainFactory domainFactory,
            IProvider domainProvider
            )
        {
            _repository = repository;
            _domainFactory = domainFactory;
            _authenticationRepository = authentication;
            _domainProvider = domainProvider;
        }


        //=========================================================================================================
        //=========================================================================================================
        //=========================================================================================================
        //Public Methods
        //DML
        public async Task<Response> CreateAsync
            (
            IEnumerable<Claim> claims,
            CreateCompanyRequestDto dto,
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
                dto.Description
                );
            await _repository.CreateAsync(domainComapany, cancellation);
            return new Response { };
        }

        public async Task<Response> UpdateAsync
            (
            IEnumerable<Claim> claims,
            UpdateCompanyRequestDto dto,
            CancellationToken cancellation
            )
        {
            var id = _authenticationRepository.GetIdNameFromClaims(claims);

            var domainCompany = await _repository.GetDomainCompanyAsync
                (
                id,
                cancellation
                );

            domainCompany.UpdateData
                (
                dto.UrlSegment,
                dto.ContactEmail,
                dto.Name,
                dto.Description
                );

            await _repository.UpdateAsync(domainCompany, cancellation);
            return new Response { };
        }
        //=========================================================================================================
        //=========================================================================================================
        //=========================================================================================================
        //Private Methods
    }
}
