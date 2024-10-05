namespace Application.VerticalSlice.AddressPart.DTOs.Select
{
    public class StreetResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public AdministrativeTypeResponseDto? AdministrativeType { get; set; } = null;
    }
}
