using Application.VerticalSlice.AddressPart.DTOs.DatabaseDTOs;

namespace Application.VerticalSlice.AddressPart.Interfaces
{
    public interface IAddressSqlClientRepository
    {
        Task<ICollection<PartialAddressDto>> GetDivisionsStreetsAsync
            (
            string administrativeDivisionName,
            string streetName,
            CancellationToken cancellation
            );
    }
}
