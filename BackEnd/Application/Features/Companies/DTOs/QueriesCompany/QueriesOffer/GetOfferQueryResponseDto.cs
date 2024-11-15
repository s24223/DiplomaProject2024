using Application.Features.Companies.DTOs.QueriesCompany.Shared;
using Application.Shared.DTOs.Features.Companies.Responses;
using Domain.Features.Offer.Entities;

namespace Application.Features.Companies.DTOs.QueriesCompany.QueriesOffer
{
    public class GetOfferQueryResponseDto
    {
        //Values
        public CompanyDetailsResponseDto Company { get; set; } = null!;
        public OfferResponseDto Offer { get; set; } = null!;
        public IEnumerable<BranchBranchOfferDetailsResponseDto> Branches { get; set; } = [];
        public int BranchesCount { get; private set; } = 0;

        //Cosntructor 
        public GetOfferQueryResponseDto(DomainOffer domain)
        {
            Offer = new OfferResponseDto(domain);

            var domainCompany = domain.BranchOffers.First().Value.Branch.Company;
            if (domainCompany != null)
            {
                Company = new CompanyDetailsResponseDto(domainCompany);
            }

            Branches = domain.BranchOffers
                .Select(x => new BranchBranchOfferDetailsResponseDto(x.Value));
            BranchesCount = Branches.Count();
        }

    }
}
