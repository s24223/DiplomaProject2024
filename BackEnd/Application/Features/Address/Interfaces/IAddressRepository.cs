using Application.Features.Address.DTOs.Select;

namespace Application.Features.Address.Interfaces
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
