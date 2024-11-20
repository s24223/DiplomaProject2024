using Application.Shared.DTOs.Features.Companies.Responses;
using Domain.Features.BranchOffer.Entities;

namespace Application.Features.Companies.Commands.BranchOffers.DTOs.CommandsBranchOffer
{
    public class CorrectBranchOfferDto
    {
        //values
        public OfferResp Offer { get; set; } = null!;
        public BranchOfferResp OfferDetails { get; set; } = null!;


        //Cosntructor
        public CorrectBranchOfferDto(DomainBranchOffer domain)
        {
            OfferDetails = new BranchOfferResp(domain);
            if (domain.Offer != null)
            {
                Offer = new OfferResp(domain.Offer);
            }
        }
    }
}
