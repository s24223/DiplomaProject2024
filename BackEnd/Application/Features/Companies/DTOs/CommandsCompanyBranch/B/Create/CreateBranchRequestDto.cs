namespace Application.Features.Companies.DTOs.CommandsCompanyBranch.B.Create
{
    public class CreateBranchRequestDto
    {
        public Guid AddressId { get; set; }
        public string? UrlSegment { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }
}
