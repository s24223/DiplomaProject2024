using Application.Features.Addresses.Queries.DTOs;
using Application.Shared.DTOs.Features.Addresses;
using Application.Shared.DTOs.Response;

namespace Application.Features.Addresses.Queries.Services
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

        Task<IEnumerable<DivisionUpResp>> GetDivisionsDownHorizontalAsync(
            int? divisionId,
            CancellationToken cancellation
            );

        Task<IEnumerable<StreetResponseDto>> GetStreetsAsync
            (
            int divisionId,
            CancellationToken cancellation
            );
    }
}
