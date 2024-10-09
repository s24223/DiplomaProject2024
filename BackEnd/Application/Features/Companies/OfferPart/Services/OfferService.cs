using Application.Database.Models;
using Application.Features.Companies.OfferPart.DTOs.Create;
using Application.Features.Companies.OfferPart.DTOs.Update;
using Application.Features.Companies.OfferPart.Interfaces;
using Application.Shared.Services.Authentication;
using Domain.Shared.Factories;
using Domain.Shared.Providers;

namespace Application.Features.Companies.OfferPart.Services
{
    public class OfferService : IOfferService
    {
        private readonly IOfferRepository _repository;
        private readonly IDomainFactory _domainFactory;
        private readonly IAuthenticationService _authenticationRepository;
        private readonly IProvider _domainProvider;


        public OfferService
            (
            IOfferRepository repository,
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

        public async Task CreateOfferProfileAsync
            (
            //IEnumerable<Claim> claims,
            CreateOfferRequestDto dto,
            CancellationToken cancellation
            )
        {
            //var id = _authenticationRepository.GetIdNameFromClaims(claims);
            var domainOffer = _domainFactory.CreateDomainOffer(
                dto.Id,
                dto.Name,
                dto.Description,
                dto.MinSalary,
                dto.MaxSalary,
                dto.NegotiatedSalary,
                dto.ForStudents

            );
            await _repository.CreateOfferProfileAsync(domainOffer, cancellation);
        }
        public async Task UpdateOfferProfileAsync
            (
            //IEnumerable<Claim> claims,
            Guid id,
            UpdateOfferRequestDto dto,
            CancellationToken cancellation
            )
        {
            //var id = _authenticationRepository.GetIdNameFromClaims(claims);
            var domainOffer = _domainFactory.CreateDomainOffer(
                id,
                dto.Name,
                dto.Description,
                dto.MinSalary,
                dto.MaxSalary,
                dto.NegotiatedSalary,
                dto.ForStudents
            );
            await _repository.CreateOfferProfileAsync(domainOffer, cancellation);
        }
    }
}
