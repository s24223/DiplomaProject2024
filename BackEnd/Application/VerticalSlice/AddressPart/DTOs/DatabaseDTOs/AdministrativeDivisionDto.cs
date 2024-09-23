namespace Application.VerticalSlice.AddressPart.DTOs.DatabaseDTOs
{
    public class AdministrativeDivisionDto
    {
        public int Id { get; set; }
        public string AdministrativeDivisionName { get; set; } = null!;
        public int? ParentDivisionId { get; set; } = null;
        public int AdministrativeTypeId { get; set; }
        public string AdministrativeTypeName { get; set; } = null!;
    }
}
