using Application.Features.Companies.Queries.QueriesOffer.DTOs.Shared;
using Application.Shared.DTOs.Features.Companies.Responses;
using Domain.Features.BranchOffer.Entities;

namespace Application.Features.Companies.Queries.QueriesOffer.DTOs.QueriesOffer
{
    public class GetBranchOfferQueryResponseDto
    {
        //Values
        public CompanyDetailsResponseDto Company { get; set; } = null!;
        public BranchResp Branch { get; set; } = null!;
        public OfferResp Offer { get; set; } = null!;
        public BranchOfferResp OfferDetails { get; set; } = null!;


        //Cosntructor
        public GetBranchOfferQueryResponseDto(DomainBranchOffer domain)
        {
            OfferDetails = new BranchOfferResp(domain);

            if (domain.Offer != null)
            {
                Offer = new OfferResp(domain.Offer);
            }
            if (domain.Branch != null)
            {
                Branch = new BranchResp(domain.Branch);
                if (domain.Branch.Company != null)
                {
                    Company = new CompanyDetailsResponseDto(domain.Branch.Company);
                }
            }

        }
    }
}
