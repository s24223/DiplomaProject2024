using Application.Features.Companies.DTOs.CommandsBranchOffer.CommandsBranchOffer.CreateBranchOffer;
using Application.Features.Companies.DTOs.CommandsBranchOffer.CommandsBranchOffer.UpdateBranchOffer;
using Application.Features.Companies.DTOs.CommandsBranchOffer.CommandsOffer.CreateOffer;
using Application.Features.Companies.DTOs.CommandsBranchOffer.CommandsOffer.UpdateOffer;
using Application.Features.Companies.Interfaces.CommandsBranchOffer;
using Application.Shared.DTOs.Features.Companies;
using Application.Shared.DTOs.Response;
using Application.Shared.Services.Authentication;
using Domain.Features.BranchOffer.Entities;
using Domain.Features.BranchOffer.ValueObjects.Identificators;
using Domain.Features.Offer.ValueObjects.Identificators;
using Domain.Features.User.ValueObjects.Identificators;
using Domain.Shared.Factories;
using System.Security.Claims;
using System.Text;

namespace Application.Features.Companies.Services.CommandsBranchOffer
{
    public class BranchOfferService : IBranchOfferService
    {
        //Values
        private readonly IBranchOfferRepository _repository;
        private readonly IDomainFactory _domainFactory;
        private readonly IAuthenticationService _authenticationRepository;


        //Cosntructor
        public BranchOfferService
            (
            IBranchOfferRepository repository,
            IDomainFactory domainFactory,
            IAuthenticationService authentication
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
        public async Task<ResponseItems<OfferResponseDto>> CreateOffersAsync
           (
           IEnumerable<Claim> claims,
           IEnumerable<CreateOfferRequestDto> dtos,
           CancellationToken cancellation
           )
        {
            var companyId = GetCompanyId(claims);
            var domainOffers = dtos.Select(dto => _domainFactory.CreateDomainOffer
                (
                dto.Name,
                dto.Description,
                dto.MinSalary,
                dto.MaxSalary,
                dto.IsNegotiatedSalary,
                dto.IsForStudents
                ));

            domainOffers = await _repository.CreateOffersAsync(companyId, domainOffers, cancellation);
            return new ResponseItems<OfferResponseDto>
            {
                Items = domainOffers.Select(x => new OfferResponseDto(x)).ToList()
            };
        }

        public async Task<ResponseItems<OfferResponseDto>> UpdateOffersAsync
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

            var dictionaryOffers = await _repository.GetOfferDictionaryAsync
                (
                companyId,
                dictionaryDtos.Keys,
                cancellation
                );

            var intersectKeys = dictionaryDtos.Keys.ToHashSet()
                .Intersect(dictionaryOffers.Keys.ToHashSet());

            foreach (var key in intersectKeys)
            {
                var dto = dictionaryDtos[key];
                var offer = dictionaryOffers[key];

                offer.Update
                        (
                        dto.Name,
                        dto.Description,
                        dto.MinSalary,
                        dto.MaxSalary,
                        dto.IsNegotiatedSalary,
                        dto.IsForStudents
                        );
            }

            await _repository.UpdateOffersAsync(companyId, dictionaryOffers, cancellation);

            return new ResponseItems<OfferResponseDto>
            {
                Items = dictionaryOffers.Select(x => new OfferResponseDto(x.Value)).ToList(),
            };
        }

        //OfferBranch Part
        public async Task<ResponseItems<CreateBranchOfferResponseDto>> CreateBranchOfferAsync
           (
           IEnumerable<Claim> claims,
           IEnumerable<CreateBranchOfferRequestDto> dtos,
           CancellationToken cancellation
           )
        {
            var companyId = GetCompanyId(claims);
            var branchOffers = dtos.Select(x => _domainFactory.CreateDomainBranchOffer
                (
                x.BranchId,
                x.OfferId,
                x.PublishStart,
                x.PublishEnd,
                x.WorkStart,
                x.WorkEnd
                ));

            var response = GenerateDuplicateErrorResponseMessage(branchOffers);
            if (response != null)
            {
                return new ResponseItems<CreateBranchOfferResponseDto>
                {
                    Status = EnumResponseStatus.UserFault,
                    Message = response,
                    Items = branchOffers.Select(x => new CreateBranchOfferResponseDto(x))
                        .ToList(),
                };
            }

            branchOffers = await _repository.CreateBranchOffersAsync
                (
                companyId,
                branchOffers,
                cancellation
                );

            return new ResponseItems<CreateBranchOfferResponseDto>
            {
                Items = branchOffers.Select(x => new CreateBranchOfferResponseDto(x)).ToList()
            };
        }

        public async Task<ResponseItems<BranchOfferResponseDto>> UpdateBranchOfferAsync
            (
            IEnumerable<Claim> claims,
            IEnumerable<UpdateBranchOfferRequestDto> dtos,
            CancellationToken cancellation
            )
        {
            var companyId = GetCompanyId(claims);
            var dtosDictionary = dtos.ToDictionary(x => new BranchOfferId(x.BranchOfferId));
            var branchOfferDictionary = await _repository.GetBranchOfferDictionaryAsync
                (
                companyId,
                dtosDictionary.Keys,
                cancellation
                );

            var intersectKeys = dtosDictionary.Keys.ToHashSet()
                .Intersect(branchOfferDictionary.Keys.ToHashSet());

            foreach (var key in intersectKeys)
            {
                var offer = branchOfferDictionary[key];
                var dto = dtosDictionary[key];
                offer.Update
                        (
                        dto.PublishStart,
                        dto.PublishEnd,
                        (DateOnly?)dto.WorkStart,
                        (DateOnly?)dto.WorkEnd
                );
            }

            var response = GenerateDuplicateErrorResponseMessage(branchOfferDictionary.Values);
            if (response != null)
            {
                return new ResponseItems<BranchOfferResponseDto>
                {
                    Status = EnumResponseStatus.UserFault,
                    Message = response,
                    Items = branchOfferDictionary.Values
                        .Select(x => new BranchOfferResponseDto(x))
                        .ToList(),
                };
            }

            await _repository.UpdateBranchOfferAsync
                    (
                    companyId,
                    branchOfferDictionary,
                    cancellation
                    );

            return new ResponseItems<BranchOfferResponseDto>
            {
                Items = branchOfferDictionary.Values
                        .Select(x => new BranchOfferResponseDto(x))
                        .ToList(),
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

        private string? GenerateDuplicateErrorResponseMessage
            (
            IEnumerable<DomainBranchOffer> branchOffers
            )
        {
            var duplicates = DomainBranchOffer.ReturnDuplicatesAndCorrectValues(branchOffers).Duplicates;
            if (!duplicates.Any())
            {
                return null;
            }

            //Duplicates Uncorrect data Message
            var builder = new StringBuilder();
            builder.AppendLine(Messages.BranchOffer_Ids_NotFound);
            builder.AppendLine("CoreId:DuplicateId");

            foreach (var (Core, Duplicate) in duplicates)
            {
                builder.AppendLine($"{Core.Id}:{Duplicate.Id}");
            }
            return builder.ToString();
        }
    }
}
