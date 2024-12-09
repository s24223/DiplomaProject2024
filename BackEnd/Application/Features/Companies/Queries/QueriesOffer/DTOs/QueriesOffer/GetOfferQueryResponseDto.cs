﻿using Application.Features.Companies.Queries.QueriesOffer.DTOs.Shared;
using Application.Shared.DTOs.Features.Companies.Responses;
using Domain.Features.Offer.Entities;

namespace Application.Features.Companies.Queries.QueriesOffer.DTOs.QueriesOffer
{
    public class GetOfferQueryResponseDto
    {
        //Values
        public CompanyDetailsResponseDto Company { get; set; } = null!;
        public OfferResp Offer { get; set; } = null!;
        public IEnumerable<BranchBranchOfferDetailsResponseDto> Branches { get; set; } = [];
        public int BranchesCount { get; private set; } = 0;

        //Cosntructor 
        public GetOfferQueryResponseDto(DomainOffer domain)
        {
            Offer = new OfferResp(domain);

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
