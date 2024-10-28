using System.ComponentModel.DataAnnotations;

namespace Application.Features.Addresses.DTOs.Commands.Create
{
    public class CreateAddressRequestDto
    {
        [Required]
        public required int DivisionId { get; set; }
        [Required]
        public required int StreetId { get; set; }
        [Required]
        public required string BuildingNumber { get; set; } = null!;
        public string? ApartmentNumber { get; set; }
        [Required]
        public required string ZipCode { get; set; } = null!;
    }
}
