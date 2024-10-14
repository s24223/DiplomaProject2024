using Application.Features.Companies.BranchOfferPart.DTOs.CreateBranchOffer;
using Application.Features.Companies.BranchOfferPart.DTOs.CreateOffer;
using Application.Features.Companies.BranchOfferPart.DTOs.UpdateBranchOffer;
using Application.Features.Companies.BranchOfferPart.DTOs.UpdateOffer;
using Application.Features.Companies.BranchOfferPart.Interfaces;
using Application.Shared.DTOs.Response;
using Application.Shared.Services.Authentication;
using Domain.Features.Branch.ValueObjects.Identificators;
using Domain.Features.BranchOffer.ValueObjects.Identificators;
using Domain.Features.Offer.ValueObjects.Identificators;
using Domain.Shared.Factories;
using System.Security.Claims;

namespace Application.Features.Companies.BranchOfferPart.Services
{
    public class BranchOfferService : IBranchOfferService
    {
        //Values
        //private readonly IProvider _domainProvider;
        private readonly IBranchOfferRepository _repository;
        private readonly IDomainFactory _domainFactory;
        private readonly IAuthenticationService _authenticationRepository;


        //Cosntructor
        public BranchOfferService
            (
            //IProvider domainProvider,
            IBranchOfferRepository repository,
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
        //DML

        //Offer part
        public async Task<ResponseItem<CreateOfferResponseDto>> CreateOfferAsync
            (
            IEnumerable<Claim> claims,
            CreateOfferRequestDto dto,
            CancellationToken cancellation
            )
        {
            var companyId = _authenticationRepository.GetIdNameFromClaims(claims);
            var domainOffer = _domainFactory.CreateDomainOffer
                (
                dto.Name,
                dto.Description,
                dto.MinSalary,
                dto.MaxSalary,
                dto.IsNegotiatedSalary,
                dto.IsForStudents
                );
            var offerId = await _repository.CreateOfferAsync(companyId, domainOffer, cancellation);
            return new ResponseItem<CreateOfferResponseDto>
            {
                Item = new CreateOfferResponseDto
                {
                    OfferId = offerId,
                },
            };
        }

        public async Task<Response> UpdateOfferAsync
            (
            IEnumerable<Claim> claims,
            Guid offerId,
            UpdateOfferRequestDto dto,
            CancellationToken cancellation
            )
        {
            var companyId = _authenticationRepository.GetIdNameFromClaims(claims);
            var domainOffer = await _repository.GetOfferAsync
                (
                companyId,
                new OfferId(offerId),
                cancellation
                );

            domainOffer.Update
                (
                dto.Name,
                dto.Description,
                dto.MinSalary,
                dto.MaxSalary,
                dto.IsNegotiatedSalary,
                dto.IsForStudents
                );

            await _repository.UpdateOfferAsync(companyId, domainOffer, cancellation);
            return new Response { };
        }

        //OfferBranch Part
        public async Task<Response> CreateBranchOfferConnectionAsync
            (
            IEnumerable<Claim> claims,
            Guid branchId,
            Guid offerId,
            CreateBranchOfferRequestDto dto,
            CancellationToken cancellation
            )
        {
            DateOnly? workStart = dto.WorkStart == null ? null : (DateOnly)dto.WorkStart;
            DateOnly? workEnd = dto.WorkEnd == null ? null : (DateOnly)dto.WorkEnd;
            var companyId = _authenticationRepository.GetIdNameFromClaims(claims);


            var domainBranchOffer = _domainFactory.CreateDomainBranchOffer
                (
                branchId,
                offerId,
                dto.PublishStart,
                dto.PublishEnd,
                workStart,
                workEnd
                );

            await _repository.CreateBranchOfferAsync
                (
                companyId,
                domainBranchOffer,
                cancellation
                );

            return new Response { };
        }

        public async Task<Response> UpdateBranchOfferConnectionAsync
            (
            IEnumerable<Claim> claims,
            Guid branchId,
            Guid offerId,
            DateTime created,
            UpdateBranchOfferRequestDto dto,
            CancellationToken cancellation
            )
        {
            DateOnly? workStart = dto.WorkStart == null ? null : (DateOnly)dto.WorkStart;
            DateOnly? workEnd = dto.WorkEnd == null ? null : (DateOnly)dto.WorkEnd;
            var companyId = _authenticationRepository.GetIdNameFromClaims(claims);
            var branchOfferId = new BranchOfferId
                    (
                    new BranchId(branchId),
                    new OfferId(offerId),
                    created
                    );
            var domainBranchOffer = await _repository.GetBranchOfferAsync
                (
                companyId,
                branchOfferId,
                cancellation
                );

            domainBranchOffer.Update
                (
                dto.PublishStart,
                dto.PublishEnd,
                workStart,
                workEnd
                );

            await _repository.UpdateBranchOfferAsync
                (
                companyId,
                domainBranchOffer,
                cancellation
                );

            return new Response { };
        }

        //====================================================================================================================
        //====================================================================================================================
        //====================================================================================================================
        //Private Methods
    }
}
