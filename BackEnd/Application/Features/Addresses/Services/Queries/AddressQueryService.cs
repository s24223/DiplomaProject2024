﻿using Application.Features.Addresses.DTOs.Queries;
using Application.Features.Addresses.Interfaces.Queries;
using Application.Shared.DTOs.Features.Addresses;
using Application.Shared.DTOs.Response;
using Domain.Features.Address.ValueObjects.Identificators;

namespace Application.Features.Addresses.Services.Queries
{
    public class AddressQueryService : IAddressQueryService
    {
        //Values
        private readonly IAddressQueryRepository _repository;


        //Cosntructor
        public AddressQueryService
            (
            IAddressQueryRepository repository
            )
        {
            _repository = repository;
        }



        //==================================================================================================
        //==================================================================================================
        //==================================================================================================
        //Public Methods
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
            DivisionId? divisionId = id.HasValue ? new DivisionId(id.Value) : null;
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