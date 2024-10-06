namespace Application.Features.Address.DTOs.Select
{
    public class DivisionResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int? ParentId { get; set; } = null;
        public AdministrativeTypeResponseDto AdministrativeType { get; set; } = null!;
    }
}
