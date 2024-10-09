namespace Application.Features.Companies.BranchOfferPart.DTOs.CreateProfile
{
    public class CreateBranchOfferDto
    {
        public Guid BranchId { get; set; }
        public Guid OfferId { get; set; }
        public DateTime PublishStart { get; set; }
        public DateTime? PublishEnd { get; set; }
        public DateOnly? WorkStart { get; set; }
        public DateOnly? WorkEnd { get; set; }
        public DateTime Created { get; } = DateTime.Now;
        public DateTime LastUpdate { get; } = DateTime.Now;
    }
}
