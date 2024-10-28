using Application.Shared.DTOs.Features.Companies;
using Domain.Features.BranchOffer.Entities;

namespace Application.Features.Companies.DTOs.CommandsBranchOffer.CommandsBranchOffer.CreateBranchOffer
{
    public class CreateBranchOfferResponseDto : BranchOfferResponseDto
    {
        //values
        public OfferResponseDto Offer { get; set; } = null!;


        //Cosntructor
        public CreateBranchOfferResponseDto(DomainBranchOffer domain) : base(domain)
        {
            if (domain.Offer != null)
            {
                Offer = new OfferResponseDto(domain.Offer);
            }
        }
    }
}
