using Application.Shared.DTOs.Features.Companies.Responses;
using Domain.Features.BranchOffer.Entities;

namespace Application.Features.Companies.Queries.QueriesOffer.DTOs.Shared
{
    public class BranchBranchOfferDetailsResponseDto
    {
        //Values
        public BranchResp Branch { get; set; } = null!;
        public BranchOfferResp OfferDetails { get; set; } = null!;


        //Constructor
        public BranchBranchOfferDetailsResponseDto(DomainBranchOffer domain)
        {
            OfferDetails = new BranchOfferResp(domain);
            if (domain.Branch != null)
            {
                Branch = new BranchResp(domain.Branch);
            }
        }
    }
}
