namespace Application.Features.Company.CompanyPart.DTOs.CreateProfile
{
    public class CreateCompanyProfileRequestDto
    {
        public string? UrlSegment { get; set; }
        public string ContactEmail { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Regon { get; set; } = null!;
        public string? Description { get; set; }

    }
}
