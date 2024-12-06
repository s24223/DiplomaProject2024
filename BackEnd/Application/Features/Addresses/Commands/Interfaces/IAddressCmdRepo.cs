using Domain.Features.Address.Entities;
using Domain.Features.Address.ValueObjects.Identificators;

namespace Application.Features.Addresses.Commands.Interfaces
{
    public interface IAddressCmdRepo
    {
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

        Task<DomainAddress> GetAddressAsync
            (
            AddressId id,
            CancellationToken cancellation
            );

    }
}
