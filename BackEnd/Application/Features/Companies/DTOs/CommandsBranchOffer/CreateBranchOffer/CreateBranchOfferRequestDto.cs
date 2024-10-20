using Application.Shared.DTOs;

namespace Application.Features.Companies.DTOs.CommandsBranchOffer.CreateBranchOffer
{
    public class CreateBranchOfferRequestDto
    {
        public DateTime PublishStart { get; set; }
        public DateTime? PublishEnd { get; set; }
        public DateOnlyRequestDto? WorkStart { get; set; }
        public DateOnlyRequestDto? WorkEnd { get; set; }
    }
}
