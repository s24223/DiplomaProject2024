namespace Application.Features.Companies.DTOs.CommandsCompanyBranch.CommandsBranch.Update
{
    public class UpdateBranchRequestDto
    {
        public Guid BranchId { get; set; }
        public Guid AddressId { get; set; }
        public string? UrlSegment { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }
}
