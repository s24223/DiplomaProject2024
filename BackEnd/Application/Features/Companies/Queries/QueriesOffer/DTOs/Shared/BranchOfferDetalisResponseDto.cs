using Application.Shared.DTOs.Features.Companies.Responses;
using Domain.Features.BranchOffer.Entities;

namespace Application.Features.Companies.Queries.QueriesOffer.DTOs.Shared
{
    public class BranchOfferDetalisResponseDto
    {
        //Values
        public OfferResp Offer { get; set; } = null!;
        public BranchOfferResp OfferDetails { get; set; } = null!;


        //Cosntructor
        public BranchOfferDetalisResponseDto(DomainBranchOffer domain)
        {
            OfferDetails = new BranchOfferResp(domain);

            if (domain.Offer != null)
            {
                Offer = new OfferResp(domain.Offer);
            }
        }
    }
}
