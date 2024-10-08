using Application.Features.Addresses.DTOs.Create;
using Application.Features.Addresses.DTOs.Select.Address;
using Application.Features.Addresses.DTOs.Select.Collocations;
using Application.Features.Addresses.DTOs.Select.Shared;
using Application.Features.Addresses.DTOs.Update;
using Application.Features.Addresses.Interfaces;
using Application.Shared.DTOs.Response;
using Application.Shared.Services.Authentication;
using Domain.Features.Address.ValueObjects;
using Domain.Shared.Factories;
using Domain.Shared.Providers;

namespace Application.Features.Addresses.Services
{
    public class AddressService : IAddressService
    {
        //Values
        private readonly IAddressRepository _repository;
        private readonly IDomainFactory _domainFactory;
        private readonly IAuthenticationService _authenticationRepository;
        private readonly IProvider _domainProvider;


        //Cosntructor
        public AddressService
            (
            IAddressRepository repository,
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



        //==================================================================================================
        //==================================================================================================
        //==================================================================================================
        //Public Methods
        //DML
        public async Task<ResponseItem<CreateAddressResponseDto>> CreateAsync
            (
            CreateAddressRequestDto dto,
            CancellationToken cancellation
            )
        {
            var adress = _domainFactory.CreateDomainAddress
                (
                dto.DivisionId,
                dto.StreetId,
                dto.BuildingNumber,
                dto.ApartmentNumber,
                dto.ZipCode
                );
            var addressId = await _repository.CreateAsync(adress, cancellation);
            return new ResponseItem<CreateAddressResponseDto>
            {
                Item = new CreateAddressResponseDto
                {
                    AddressId = addressId,
                }
            };
        }

        public async Task<Response> UpdateAsync
            (
            Guid id,
            UpdateAddressRequestDto dto,
            CancellationToken cancellation
            )
        {
            var address = await _repository.GetAddressAsync(id, cancellation);
            address.ZipCode = new ZipCode(dto.ZipCode);

            await _repository.UpdateAsync(address, cancellation);
            return new Response { };
        }

        //DQL
        public async Task<ResponseItems<CollocationResponseDto>> GetCollocationsAsync
            (
            string divisionName,
            string streetName,
            CancellationToken cancellation
            )
        {
            var collocations = await _repository
                .GetCollocationsAsync(divisionName, streetName, cancellation);
            return new ResponseItems<CollocationResponseDto>
            {
                Items = collocations.ToList()
            };
        }

        public async Task<ResponseItem<GetAddressResponseDto>> GetAddressAsync
            (
            Guid id,
            CancellationToken cancellation
            )
        {
            var address = await _repository.GetAddressAsync(id, cancellation);

            return new ResponseItem<GetAddressResponseDto>
            {
                Item = new GetAddressResponseDto
                {
                    DivisionId = address.DivisionId.Value,
                    StreetId = address.StreetId.Value,
                    BuildingNumber = address.BuildingNumber.Value,
                    ApartmentNumber = address.ApartmentNumber == null ?
                    null : address.ApartmentNumber.Value,
                    ZipCode = address.ZipCode.Value,
                    Street = new StreetResponseDto
                    {
                        Id = address.Street.Id.Value,
                        Name = address.Street.Name,
                        AdministrativeType = address.Street.StreetType == null ?
                        null : new AdministrativeTypeResponseDto
                        {
                            Id = address.Street.StreetType.Id,
                            Name = address.Street.StreetType.Name,
                        }
                    },
                    Hierarchy = address.Hierarchy.Select(x => new DivisionResponseDto
                    {
                        Id = x.Id.Value,
                        Name = x.Name,
                        ParentId = x.ParentDivisionId,
                        AdministrativeType = new AdministrativeTypeResponseDto
                        {
                            Id = x.DivisionType.Id,
                            Name = x.DivisionType.Name,
                        }
                    }).ToList(),
                }
            };
        }

        //==================================================================================================
        //==================================================================================================
        //==================================================================================================
        //Private Methods
    }
}
