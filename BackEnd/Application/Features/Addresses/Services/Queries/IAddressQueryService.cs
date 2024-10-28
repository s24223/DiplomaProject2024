﻿using Application.Features.Addresses.DTOs.Queries;
using Application.Shared.DTOs.Features.Addresses;
using Application.Shared.DTOs.Response;

namespace Application.Features.Addresses.Services.Queries
{
    public interface IAddressQueryService
    {
        //DQL
        Task<ResponseItems<CollocationResponseDto>> GetCollocationsAsync
            (
            string divisionName,
            string streetName,
            CancellationToken cancellation
            );

        Task<ResponseItem<AddressResponseDto>> GetAddressAsync
            (
            Guid id,
            CancellationToken cancellation
            );

        Task<ResponseItems<DivisionStreetsResponseDto>> GetDivisionsDownAsync
            (
            int? id,
            CancellationToken cancellation
            );
    }
}
