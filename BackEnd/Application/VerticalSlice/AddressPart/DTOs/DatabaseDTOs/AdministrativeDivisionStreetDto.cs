namespace Application.VerticalSlice.AddressPart.DTOs.DatabaseDTOs
{
    public class AdministrativeDivisionStreetDTO
    {
        public int AdministrativeDivisionId { get; set; }
        public int StreetId { get; set; }
        public string StreetName { get; set; } = null!;
        public int StreetAdministrativeTypeId { get; set; }
        public string StreetAdministrativeTypeName { get; set; } = null!;
    }
}
