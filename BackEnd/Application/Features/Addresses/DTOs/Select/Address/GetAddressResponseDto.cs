using Application.Features.Addresses.DTOs.Select.Shared;

namespace Application.Features.Addresses.DTOs.Select.Address
{
    public class GetAddressResponseDto
    {
        public required int DivisionId { get; set; }
        public IEnumerable<DivisionResponseDto> Hierarchy { get; set; } = new List<DivisionResponseDto>();
        public required int StreetId { get; set; }
        public required StreetResponseDto Street { get; set; }
        public required string BuildingNumber { get; set; } = null!;
        public string? ApartmentNumber { get; set; }
        public required string ZipCode { get; set; } = null!;
    }
}
