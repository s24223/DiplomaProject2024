using Application.Features.Addresses.Queries.DTOs;
using Domain.Features.Address.Entities;
using Domain.Features.Address.ValueObjects.Identificators;

namespace Application.Features.Addresses.Queries.Interfaces
{
    public interface IAddressQueryRepository
    {
        Task<IEnumerable<CollocationResponseDto>> GetCollocationsAsync
            (
            string divisionName,
            string streetName,
            CancellationToken cancellation
            );

        Task<DomainAddress> GetAddressAsync
            (
            AddressId id,
            CancellationToken cancellation
            );

        Task<Dictionary<AddressId, DomainAddress>> GetAddressDictionaryAsync
          (
          IEnumerable<AddressId> ids,
          CancellationToken cancellation
          );

        Task<IEnumerable<DivisionStreetsResponseDto>> GetDivisionsDownAsync
            (
            DivisionId? id,
            CancellationToken cancellation
            );
    }
}
