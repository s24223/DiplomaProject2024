using Application.Features.Companies.DTOs.CommandsCompanyBranch.CommandsBranch.Create;
using System.ComponentModel.DataAnnotations;

namespace Application.Features.Companies.DTOs.CommandsCompanyBranch.CommandsCompany.Create
{
    public class CreateCompanyRequestDto
    {
        public string? UrlSegment { get; set; }
        [Required]
        [EmailAddress]
        public required string ContactEmail { get; set; } = null!;
        [Required]
        public required string Name { get; set; } = null!;
        [Required]
        public required string Regon { get; set; } = null!;
        public string? Description { get; set; }
        public IEnumerable<CreateBranchRequestDto> Branches { get; set; } = [];
    }
}
