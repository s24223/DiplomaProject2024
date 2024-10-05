namespace Application.VerticalSlice.AddressPart.DTOs.Select
{
    public class CollocationResponseDto
    {
        public IEnumerable<DivisionResponseDto> Hierarchy { get; set; } = new List<DivisionResponseDto>();
        public StreetResponseDto Street { get; set; } = null!;
    }
}
