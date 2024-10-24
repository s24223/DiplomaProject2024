﻿using Application.Features.Addresses.DTOs.Create;
using Application.Features.Addresses.DTOs.Select;
using Application.Features.Addresses.DTOs.Update;
using Application.Features.Addresses.Interfaces;
using Application.Shared.DTOs.Addresses;
using Application.Shared.DTOs.Response;
using Domain.Features.Address.ValueObjects.Identificators;
using Domain.Shared.Factories;
using Domain.Shared.Providers;

namespace Application.Features.Addresses.Services
{
    public class AddressService : IAddressService
    {
        //Values
        private readonly IAddressRepository _repository;
        private readonly IDomainFactory _domainFactory;
        private readonly IProvider _domainProvider;


        //Cosntructor
        public AddressService
            (
            IAddressRepository repository,
            IDomainFactory domainFactory,
            IProvider domainProvider
            )
        {
            _repository = repository;
            _domainFactory = domainFactory;
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
            var address = await _repository.GetAddressAsync(new AddressId(id), cancellation);
            address.SetZipCode(dto.ZipCode);
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
            var collocations = await _repository.GetCollocationsAsync(divisionName, streetName, cancellation);
            return new ResponseItems<CollocationResponseDto>
            {
                Items = collocations.ToList()
            };
        }

        public async Task<ResponseItem<AddressResponseDto>> GetAddressAsync
            (
            Guid id,
            CancellationToken cancellation
            )
        {
            var address = await _repository.GetAddressAsync(new AddressId(id), cancellation);
            return new ResponseItem<AddressResponseDto>
            {
                Item = new AddressResponseDto(address),
            };
        }

        public async Task<ResponseItems<DivisionStreetsResponseDto>> GetDivisionsDownAsync
            (
            int? id,
            CancellationToken cancellation
            )
        {
            DivisionId? divisionId = (id.HasValue ? new DivisionId(id.Value) : null);
            var items = await _repository.GetDivisionsDownAsync(divisionId, cancellation);

            return new ResponseItems<DivisionStreetsResponseDto>
            {
                Items = items.ToList(),
            };
        }
        //==================================================================================================
        //==================================================================================================
        //==================================================================================================
        //Private Methods
    }
}
