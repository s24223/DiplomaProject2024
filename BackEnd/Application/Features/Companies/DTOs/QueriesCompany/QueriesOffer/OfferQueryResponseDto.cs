using Application.Shared.DTOs.Features.Companies;
using Domain.Features.BranchOffer.Entities;

namespace Application.Features.Companies.DTOs.QueriesCompany.QueriesOffer
{
    public class OfferQueryResponseDto : BranchOfferResponseDto
    {
        //UrlResponseDto
        //CompanyResponseDto
        public BranchDetailsQueryResponseDto Branch { get; set; } = null!;
        public OfferResponseDto Offer { get; set; } = null!;

        public OfferQueryResponseDto(DomainBranchOffer domain) : base(domain)
        {
            if (domain.Offer != null)
            {
                Offer = new OfferResponseDto(domain.Offer);
            }
            if (domain.Branch != null)
            {
                Branch = new BranchDetailsQueryResponseDto(domain.Branch);
            }
        }
    }
}
