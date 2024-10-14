using Application.Shared.DTOs;

namespace Application.Features.Companies.BranchOfferPart.DTOs.UpdateBranchOffer
{
    public class UpdateBranchOfferRequestDto
    {
        public DateTime PublishStart { get; set; }
        public DateTime? PublishEnd { get; set; }
        public DateOnlyRequestDto? WorkStart { get; set; }
        public DateOnlyRequestDto? WorkEnd { get; set; }
    }
}
