using Application.Features.Companies.CompanyPart.DTOs.Create;
using Application.Features.Companies.CompanyPart.DTOs.Update;
using Application.Features.Companies.CompanyPart.Interfaces;
using Application.Shared.DTOs.Response;
using Application.Shared.Services.Authentication;
using Domain.Shared.Factories;
using System.Security.Claims;

namespace Application.Features.Companies.CompanyPart.Services
{
    public class CompanyService : ICompanyService
    {
        //Values
        //private readonly IProvider _domainProvider;
        private readonly IDomainFactory _domainFactory;
        private readonly ICompanyRepository _repository;
        private readonly IAuthenticationService _authenticationRepository;

        //Cosntructor
        public CompanyService
            (
            //IProvider domainProvider,
            IDomainFactory domainFactory,
            ICompanyRepository repository,
            IAuthenticationService authentication
            )
        {
            //_domainProvider = domainProvider;
            _repository = repository;
            _domainFactory = domainFactory;
            _authenticationRepository = authentication;
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

            var domainCompany = await _repository.GetCompanyAsync
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
