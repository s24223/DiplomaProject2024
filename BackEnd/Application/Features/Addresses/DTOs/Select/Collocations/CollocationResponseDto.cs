using Application.Features.Addresses.DTOs.Select.Shared;

namespace Application.Features.Addresses.DTOs.Select.Collocations
{
    public class CollocationResponseDto
    {
        public IEnumerable<DivisionResponseDto> Hierarchy { get; set; } = new List<DivisionResponseDto>();
        public StreetResponseDto Street { get; set; } = null!;
    }
}
