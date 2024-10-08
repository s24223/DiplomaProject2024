namespace Application.Features.Addresses.DTOs.Select.Shared
{
    public class StreetResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public AdministrativeTypeResponseDto? AdministrativeType { get; set; } = null;
    }
}
