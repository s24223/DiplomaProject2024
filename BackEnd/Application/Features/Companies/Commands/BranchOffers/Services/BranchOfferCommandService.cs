using Application.Features.Companies.Commands.BranchOffers.DTOs.CommandsBranchOffer.CreateBranchOffer.Request;
using Application.Features.Companies.Commands.BranchOffers.DTOs.CommandsBranchOffer.CreateBranchOffer.Response;
using Application.Features.Companies.Commands.BranchOffers.DTOs.CommandsBranchOffer.UpdateBranchOffer.Request;
using Application.Features.Companies.Commands.BranchOffers.DTOs.CommandsBranchOffer.UpdateBranchOffer.Response;
using Application.Features.Companies.Commands.BranchOffers.DTOs.CommandsOffer.CreateOffer.Request;
using Application.Features.Companies.Commands.BranchOffers.DTOs.CommandsOffer.UpdateOffer.Request;
using Application.Features.Companies.Commands.BranchOffers.Interfaces;
using Application.Shared.DTOs.Features.Companies.Responses;
using Application.Shared.DTOs.Response;
using Application.Shared.Services.Authentication;
using Domain.Features.BranchOffer.Entities;
using Domain.Features.BranchOffer.ValueObjects.Identificators;
using Domain.Features.Characteristic.ValueObjects.Identificators;
using Domain.Features.Offer.ValueObjects.Identificators;
using Domain.Features.User.ValueObjects.Identificators;
using Domain.Shared.Factories;
using System.Security.Claims;

namespace Application.Features.Companies.Commands.BranchOffers.Services
{
    public class BranchOfferCommandService : IBranchOfferCommandService
    {
        //Values
        private readonly IBranchOfferRepository _repository;
        private readonly IDomainFactory _domainFactory;
        private readonly IAuthJwtSvc _authenticationRepository;


        //Cosntructor
        public BranchOfferCommandService
            (
            IBranchOfferRepository repository,
            IDomainFactory domainFactory,
            IAuthJwtSvc authentication
            )
        {
            _repository = repository;
            _domainFactory = domainFactory;
            _authenticationRepository = authentication;
        }


        //====================================================================================================================
        //====================================================================================================================
        //====================================================================================================================
        //Public Methods
        //DML

        //Offer part
        public async Task<ResponseItems<OfferResp>> CreateOffersAsync
           (
           IEnumerable<Claim> claims,
           IEnumerable<CreateOfferRequestDto> dtos,
           CancellationToken cancellation
           )
        {
            var companyId = GetCompanyId(claims);
            var domainOffers = dtos.Select(dto =>
            {
                var characteristics = dto.Characteristics
                .Select(x => ((CharacteristicId, QualityId?))x);

                var domainOffer = _domainFactory.CreateDomainOffer(
                    dto.Name,
                    dto.Description,
                    dto.MinSalary,
                    dto.MaxSalary,
                    dto.IsNegotiatedSalary,
                    dto.IsForStudents
                    );
                domainOffer.SetCharacteristics(characteristics);
                return domainOffer;
            });

            domainOffers = await _repository
                .CreateOffersAsync(companyId, domainOffers, cancellation);

            return new ResponseItems<OfferResp>
            {
                Items = domainOffers
                    .Select(x => new OfferResp(x))
                    .ToList()
            };
        }

        public async Task<ResponseItems<OfferResp>> UpdateOffersAsync
            (
            IEnumerable<Claim> claims,
            IEnumerable<UpdateOfferRequestDto> dtos,
            CancellationToken cancellation
            )
        {
            var companyId = GetCompanyId(claims);
            var dictionaryDtos = new Dictionary<OfferId, UpdateOfferRequestDto>();
            foreach (var dto in dtos)
            {
                var id = new OfferId(dto.OfferId);
                dictionaryDtos[id] = dto;
            }


            var dictionaryOffers = await _repository
                .GetOfferDictionaryAsync(companyId, dictionaryDtos.Keys, cancellation);

            foreach (var key in dictionaryDtos.Keys)
            {
                var dto = dictionaryDtos[key];
                var offer = dictionaryOffers[key];
                var characteristics = dto.Characteristics
                    .Select(x => ((CharacteristicId, QualityId?))x);

                offer.Update
                        (
                        dto.Name,
                        dto.Description,
                        dto.MinSalary,
                        dto.MaxSalary,
                        dto.IsNegotiatedSalary,
                        dto.IsForStudents
                        );
                offer.SetCharacteristics(characteristics);
            }

            dictionaryOffers = await _repository
                .UpdateOffersAsync(companyId, dictionaryOffers, cancellation);

            return new ResponseItems<OfferResp>
            {
                Items = dictionaryOffers
                    .Select(x => new OfferResp(x.Value))
                    .ToList(),
            };
        }

        //OfferBranch Part
        public async Task<ResponseItem<CreateBranchOffersResponseDto>> CreateBranchOffersAsync
           (
           IEnumerable<Claim> claims,
           IEnumerable<CreateBranchOfferRequestDto> dtos,
           CancellationToken cancellation
           )
        {
            var companyId = GetCompanyId(claims);
            var branchOffers = dtos.Select(x => _domainFactory.CreateDomainBranchOffer(
                x.BranchId,
                x.OfferId,
                x.PublishStart,
                x.PublishEnd,
                x.WorkStart,
                x.WorkEnd
                ));

            var values = DomainBranchOffer.SeparateAndFilterBranchOffers(branchOffers);
            branchOffers = values.Correct;

            if (branchOffers.Count() != dtos.Count())
            {
                return new ResponseItem<CreateBranchOffersResponseDto>
                {
                    Status = EnumResponseStatus.UserFault,
                    Item = new CreateBranchOffersResponseDto(
                        [],
                        values.WithoutDuration,
                        values.Conflicts
                        ),
                };
            }

            branchOffers = await _repository.CreateBranchOffersAsync
                (
                companyId,
                branchOffers,
                cancellation
                );

            return new ResponseItem<CreateBranchOffersResponseDto>
            {
                Item = new CreateBranchOffersResponseDto(
                        branchOffers,
                        values.WithoutDuration,
                        values.Conflicts
                        ),
            };
        }

        public async Task<ResponseItem<UpdateBranchOfferResponseDto>> UpdateBranchOffersAsync
            (
            IEnumerable<Claim> claims,
            IEnumerable<UpdateBranchOfferRequestDto> dtos,
            CancellationToken cancellation
            )
        {
            var companyId = GetCompanyId(claims);
            //DTOs To Dictionary
            var dtosDictionary = new Dictionary<BranchOfferId, UpdateBranchOfferRequestDto>();
            foreach (var dto in dtos)
            {
                var key = new BranchOfferId(dto.BranchOfferId);
                dtosDictionary[key] = dto;
            }

            //Get From Database
            var dictionary = await _repository.GetBranchOfferDictionaryAsync
                (
                companyId,
                dtosDictionary.Keys,
                cancellation
                );

            //Update them
            foreach (var key in dtosDictionary.Keys)
            {
                var offer = dictionary[key];
                var dto = dtosDictionary[key];

                offer.Update
                    (
                    dto.PublishStart,
                    dto.PublishEnd,
                    (DateOnly?)dto.WorkStart,
                    (DateOnly?)dto.WorkEnd
                );
            }

            //Check Conflicts
            var values = DomainBranchOffer.SeparateAndFilterBranchOffers(dictionary.Values);
            if (values.Correct.Count() != dictionary.Count())
            {
                return new ResponseItem<UpdateBranchOfferResponseDto>
                {
                    Item = new UpdateBranchOfferResponseDto
                        (
                        [],
                        values.WithoutDuration,
                        values.Conflicts
                        ),
                };
            }

            //update Database
            dictionary = await _repository.UpdateBranchOfferAsync(companyId, dictionary, cancellation);

            return new ResponseItem<UpdateBranchOfferResponseDto>
            {
                Item = new UpdateBranchOfferResponseDto
                        (
                        dictionary.Values,
                        values.WithoutDuration,
                        values.Conflicts
                        ),
            };
        }
        //====================================================================================================================
        //====================================================================================================================
        //====================================================================================================================
        //Private Methods
        private UserId GetCompanyId(IEnumerable<Claim> claims)
        {
            return _authenticationRepository.GetIdNameFromClaims(claims);
        }

    }
}
