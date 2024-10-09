using Application.Features.Addresses.DTOs.Create;
using Application.Features.Addresses.DTOs.Select.Address;
using Application.Features.Addresses.DTOs.Select.Collocations;
using Application.Features.Addresses.DTOs.Select.Shared;
using Application.Features.Addresses.DTOs.Update;
using Application.Shared.DTOs.Response;

namespace Application.Features.Addresses.Services
{
    public interface IAddressService
    {
        //DML
        Task<ResponseItem<CreateAddressResponseDto>> CreateAsync
            (
            CreateAddressRequestDto dto,
            CancellationToken cancellation
            );

        Task<Response> UpdateAsync
            (
            Guid id,
            UpdateAddressRequestDto dto,
            CancellationToken cancellation
            );

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

        Task<ResponseItems<DivisionResponseDto>> GetDivisionsDownAsync
            (
            int? id,
            CancellationToken cancellation
            );
    }
}
