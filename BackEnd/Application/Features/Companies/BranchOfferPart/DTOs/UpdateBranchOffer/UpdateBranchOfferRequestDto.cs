namespace Application.Features.Companies.BranchOfferPart.DTOs.UpdateBranchOffer
{
    public class UpdateBranchOfferRequestDto
    {
        public DateTime PublishStart { get; set; }
        public DateTime? PublishEnd { get; set; }
        public DateOnly? WorkStart { get; set; }
        public DateOnly? WorkEnd { get; set; }
    }
}
