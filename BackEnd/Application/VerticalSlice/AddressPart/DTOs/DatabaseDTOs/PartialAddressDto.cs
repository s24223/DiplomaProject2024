namespace Application.VerticalSlice.AddressPart.DTOs.DatabaseDTOs
{
    public class PartialAddressDto
    {
        public List<AdministrativeDivisionDto> Hierarchy { get; set; } = new();
        public int StreetId { get; set; }
        public string StreetName { get; set; } = null!;
        public int StreetAdministrativeTypeId { get; set; }
        public string StreetAdministrativeTypeName { get; set; } = null!;
    }
}
