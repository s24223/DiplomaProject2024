using Domain.Features.Address.Entities;
using Domain.Features.Address.ValueObjects.Identificators;

namespace Application.Features.Addresses.Queries.Interfaces
{
    public interface IAddressQueryRepo
    {
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

        /*Task<IEnumerable<CollocationResponseDto>> GetCollocationsAsync
            (
            string divisionName,
            string streetName,
            CancellationToken cancellation
            );        

        Task<IEnumerable<DivisionStreetsResponseDto>> GetDivisionsDownVerticalAsync
            (
            DivisionId? id,
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
            );*/
    }
}
