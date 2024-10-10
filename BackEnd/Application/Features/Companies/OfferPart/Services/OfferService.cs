using Application.Features.Companies.OfferPart.DTOs.Create;
using Application.Features.Companies.OfferPart.DTOs.Update;
using Application.Features.Companies.OfferPart.Interfaces;
using Application.Shared.DTOs.Response;
using Application.Shared.Services.Authentication;
using Domain.Features.Offer.ValueObjects.Identificators;
using Domain.Shared.Factories;
using System.Security.Claims;

namespace Application.Features.Companies.OfferPart.Services
{
    public class OfferService : IOfferService
    {
        //Values
        //private readonly IProvider _domainProvider;
        private readonly IOfferRepository _repository;
        private readonly IDomainFactory _domainFactory;
        private readonly IAuthenticationService _authenticationRepository;


        //Cosntructor
        public OfferService
            (
            //IProvider domainProvider,
            IOfferRepository repository,
            IDomainFactory domainFactory,
            IAuthenticationService authentication
            )
        {
            _repository = repository;
            _domainFactory = domainFactory;
            //_domainProvider = domainProvider;
            _authenticationRepository = authentication;
        }



        //====================================================================================================================
        //====================================================================================================================
        //====================================================================================================================
        //Public Methods
        public async Task<ResponseItem<CreateOfferResponseDto>> CreateAsync
            (
            IEnumerable<Claim> claims,
            CreateOfferRequestDto dto,
            CancellationToken cancellation
            )
        {
            var companyId = _authenticationRepository.GetIdNameFromClaims(claims);
            var doaminOffer = _domainFactory.CreateDomainOffer
                (
                dto.Name,
                dto.Description,
                dto.MinSalary,
                dto.MaxSalary,
                dto.IsNegotiatedSalary,
                dto.IsForStudents
                );
            var offerId = await _repository.CreateAsync(companyId, doaminOffer, cancellation);
            return new ResponseItem<CreateOfferResponseDto>
            {
                Item = new CreateOfferResponseDto
                {
                    OfferId = offerId,
                },
            };
        }

        public async Task<Response> UpdateAsync
            (
            IEnumerable<Claim> claims,
            Guid offerId,
            UpdateOfferRequestDto dto,
            CancellationToken cancellation
            )
        {
            var companyId = _authenticationRepository.GetIdNameFromClaims(claims);
            var domainOffer = await _repository.GetOfferAsync(companyId, new OfferId(offerId), cancellation);
            domainOffer.Update
                (
                dto.Name,
                dto.Description,
                dto.MinSalary,
                dto.MaxSalary,
                dto.IsNegotiatedSalary,
                dto.IsForStudents
                );

            await _repository.UpdateAsync(companyId, domainOffer, cancellation);
            return new Response { };
        }
        //====================================================================================================================
        //====================================================================================================================
        //====================================================================================================================
        //Private Methods
    }
}
