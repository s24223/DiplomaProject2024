namespace Application.Features.Address.DTOs.Create
{
    public class CreateAddressRequestDto
    {
        public int DivisionId { get; set; }
        public int StreetId { get; set; }
        public string BuildingNumber { get; set; } = null!;
        public string? ApartmentNumber { get; set; }
        public string ZipCode { get; set; } = null!;
    }
}
