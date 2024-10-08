using System.ComponentModel.DataAnnotations;

namespace Application.Features.Company.CompanyPart.DTOs.Create
{
    public class CreateCompanyRequestDto
    {
        public string? UrlSegment { get; set; }
        [Required]
        public required string ContactEmail { get; set; } = null!;
        [Required]
        public required string Name { get; set; } = null!;
        [Required]
        public required string Regon { get; set; } = null!;
        public string? Description { get; set; }
    }
}
