namespace Application.Features.Addresses.DTOs.Select.Shared
{
    public class DivisionResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int? ParentId { get; set; } = null;
        public AdministrativeTypeResponseDto AdministrativeType { get; set; } = null!;
    }
}
