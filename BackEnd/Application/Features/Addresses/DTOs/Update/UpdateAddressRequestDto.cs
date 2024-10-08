using System.ComponentModel.DataAnnotations;

namespace Application.Features.Addresses.DTOs.Update
{
    public class UpdateAddressRequestDto
    {
        [Required]
        public required string ZipCode { get; set; } = null!;
    }
}
