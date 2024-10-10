namespace Application.Features.Companies.BranchOfferPart.DTOs.CreateBranchOffer
{
    public class CreateBranchOfferRequestDto
    {
        public DateTime PublishStart { get; set; }
        public DateTime? PublishEnd { get; set; }
        public DateOnly? WorkStart { get; set; }
        public DateOnly? WorkEnd { get; set; }
    }
}
