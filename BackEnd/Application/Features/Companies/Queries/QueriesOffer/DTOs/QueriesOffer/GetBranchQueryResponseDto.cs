using Application.Features.Companies.Queries.QueriesOffer.DTOs.Shared;
using Application.Shared.DTOs.Features.Companies.Responses;
using Domain.Features.Branch.Entities;

namespace Application.Features.Companies.Queries.QueriesOffer.DTOs.QueriesOffer
{
    public class GetBranchQueryResponseDto
    {
        //Values
        public CompanyDetailsResponseDto Company { get; set; } = null!;
        public BranchResponseDto Branch { get; set; } = null!;
        public IEnumerable<BranchOfferDetalisResponseDto> Offers { get; set; } = [];
        public int OffersCount { get; private set; } = 0;


        //Cosntructor
        public GetBranchQueryResponseDto(DomainBranch domain)
        {
            Branch = new BranchResponseDto(domain);

            if (domain.Company != null)
            {
                Company = new CompanyDetailsResponseDto(domain.Company);
            }
            if (domain.BranchOffers.Any())
            {
                OffersCount = domain.BranchOffers.Count();
                Offers = domain.BranchOffers.Select(x =>
                    new BranchOfferDetalisResponseDto(x.Value)
                    ).ToList();
            }
        }
    }
}
