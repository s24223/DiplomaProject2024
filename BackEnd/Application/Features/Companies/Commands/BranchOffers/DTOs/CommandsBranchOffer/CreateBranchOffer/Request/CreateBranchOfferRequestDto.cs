using Application.Shared.DTOs;
using System.ComponentModel.DataAnnotations;

namespace Application.Features.Companies.Commands.BranchOffers.DTOs.CommandsBranchOffer.CreateBranchOffer.Request
{
    public class CreateBranchOfferRequestDto
    {
        //Values
        [Required]
        public required Guid BranchId { get; set; }
        [Required]
        public required Guid OfferId { get; set; }
        [Required]
        public required DateTime PublishStart { get; set; }
        public DateTime? PublishEnd { get; set; }
        public DateOnlyRequestDto? WorkStart { get; set; }
        public DateOnlyRequestDto? WorkEnd { get; set; }
    }
}
