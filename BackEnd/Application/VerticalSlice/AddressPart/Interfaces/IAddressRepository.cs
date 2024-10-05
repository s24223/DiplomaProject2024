using Application.VerticalSlice.AddressPart.DTOs.Select;

namespace Application.VerticalSlice.AddressPart.Interfaces
{
    public interface IAddressRepository
    {
        Task<IEnumerable<CollocationResponseDto>> GetCollocationsAsync
            (
            string divisionName,
            string streetName,
            CancellationToken cancellation
            );
    }
}
