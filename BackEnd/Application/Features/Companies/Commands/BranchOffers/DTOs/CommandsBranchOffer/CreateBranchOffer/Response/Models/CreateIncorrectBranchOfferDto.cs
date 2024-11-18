using Domain.Features.BranchOffer.Entities;

namespace Application.Features.Companies.Commands.BranchOffers.DTOs.CommandsBranchOffer.CreateBranchOffer.Response.Models
{
    public class CreateIncorrectBranchOfferDto
    {
        //Values
        public Guid BranchId { get; set; }
        public Guid OfferId { get; set; }
        public DateTime PublishStart { get; set; }
        public DateTime? PublishEnd { get; set; }
        public DateOnly? WorkStart { get; set; }
        public DateOnly? WorkEnd { get; set; }


        //Constructor
        public CreateIncorrectBranchOfferDto(DomainBranchOffer domain)
        {
            BranchId = domain.BranchId.Value;
            OfferId = domain.OfferId.Value;
            PublishStart = domain.PublishStart;
            PublishEnd = domain.PublishEnd;
            WorkStart = domain.WorkStart;
            WorkEnd = domain.WorkEnd;
        }
    }
}
