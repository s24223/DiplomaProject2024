using Application.Shared.DTOs.Features.Companies.Responses;
using Domain.Features.BranchOffer.Entities;

namespace Application.Features.Companies.DTOs.QueriesCompany.Shared
{
    public class BranchBranchOfferDetailsResponseDto
    {
        //Values
        public BranchResponseDto Branch { get; set; } = null!;
        public BranchOfferResponseDto OfferDetails { get; set; } = null!;


        //Constructor
        public BranchBranchOfferDetailsResponseDto(DomainBranchOffer domain)
        {
            OfferDetails = new BranchOfferResponseDto(domain);
            if (domain.Branch != null)
            {
                Branch = new BranchResponseDto(domain.Branch);
            }
        }
    }
}
