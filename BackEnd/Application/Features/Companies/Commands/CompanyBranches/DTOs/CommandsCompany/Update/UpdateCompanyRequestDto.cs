using System.ComponentModel.DataAnnotations;

namespace Application.Features.Companies.Commands.CompanyBranches.DTOs.CommandsCompany.Update
{
    public class UpdateCompanyRequestDto
    {
        public string? UrlSegment { get; set; }
        [Required]
        [EmailAddress]
        public required string ContactEmail { get; set; } = null!;
        [Required]
        public required string Name { get; set; } = null!;
        public string? Description { get; set; }
    }
}
