using Application.Features.Addresses.DTOs.Select.Collocations;
using Domain.Features.Address.Entities;

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
            Guid id,
            CancellationToken cancellation
            );
    }
}
