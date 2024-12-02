namespace Application.Features.Companies.Commands.CompanyBranches.DTOs.CommandsBranch.Create
{
    public class CreateBranchReq
    {
        public Guid AddressId { get; set; }
        public string? UrlSegment { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }
}
