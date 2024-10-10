namespace Application.Features.Companies.BranchPart.DTOs.Update
{
    public class UpdateBranchRequestDto
    {
        public Guid AddressId { get; set; }
        public string? UrlSegment { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }
}
