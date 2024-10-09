using Application.Features.Addresses.DTOs.Select.Collocations;
using Application.Features.Addresses.DTOs.Select.Shared;
using Domain.Features.Address.Entities;
using Domain.Features.Address.ValueObjects.Identificators;

namespace Application.Features.Addresses.Interfaces
{
    public interface IAddressRepository
    {
        //DML
        Task<Guid> CreateAsync
            (
            DomainAddress address,
            CancellationToken cancellation
            );

        Task UpdateAsync
            (
            DomainAddress address,
            CancellationToken cancellation
            );

        //DQL
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

        Task<IEnumerable<DivisionResponseDto>> GetDivisionsDownAsync
            (
            DivisionId? id,
            CancellationToken cancellation
            );
    }
}
