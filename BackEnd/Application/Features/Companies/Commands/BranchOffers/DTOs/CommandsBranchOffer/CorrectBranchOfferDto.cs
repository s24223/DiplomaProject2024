using Application.Shared.DTOs.Features.Companies.Responses;
using Domain.Features.BranchOffer.Entities;

namespace Application.Features.Companies.Commands.BranchOffers.DTOs.CommandsBranchOffer
{
    public class CorrectBranchOfferDto
    {
        //values
        public OfferResponseDto Offer { get; set; } = null!;
        public BranchOfferResponseDto OfferDetails { get; set; } = null!;


        //Cosntructor
        public CorrectBranchOfferDto(DomainBranchOffer domain)
        {
            OfferDetails = new BranchOfferResponseDto(domain);
            if (domain.Offer != null)
            {
                Offer = new OfferResponseDto(domain.Offer);
            }
        }
    }
}
