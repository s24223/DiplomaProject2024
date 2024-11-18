using Application.Features.Companies.Queries.QueriesOffer.DTOs.Shared;
using Application.Shared.DTOs.Features.Companies.Responses;
using Domain.Features.BranchOffer.Entities;

namespace Application.Features.Companies.Queries.QueriesOffer.DTOs.QueriesOffer
{
    public class GetBranchOfferQueryResponseDto
    {
        //Values
        public CompanyDetailsResponseDto Company { get; set; } = null!;
        public BranchResponseDto Branch { get; set; } = null!;
        public OfferResponseDto Offer { get; set; } = null!;
        public BranchOfferResponseDto OfferDetails { get; set; } = null!;


        //Cosntructor
        public GetBranchOfferQueryResponseDto(DomainBranchOffer domain)
        {
            OfferDetails = new BranchOfferResponseDto(domain);

            if (domain.Offer != null)
            {
                Offer = new OfferResponseDto(domain.Offer);
            }
            if (domain.Branch != null)
            {
                Branch = new BranchResponseDto(domain.Branch);
                if (domain.Branch.Company != null)
                {
                    Company = new CompanyDetailsResponseDto(domain.Branch.Company);
                }
            }

        }
    }
}
