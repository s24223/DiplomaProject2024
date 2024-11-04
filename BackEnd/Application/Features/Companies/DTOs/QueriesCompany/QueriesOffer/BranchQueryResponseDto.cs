﻿using Application.Features.Companies.DTOs.QueriesCompany.Shared;
using Application.Shared.DTOs.Features.Companies;
using Domain.Features.Branch.Entities;

namespace Application.Features.Companies.DTOs.QueriesCompany.QueriesOffer
{
    public class BranchQueryResponseDto
    {
        //Values
        public CompanyDetailsResponseDto Company { get; set; } = null!;
        public BranchResponseDto Branch { get; set; } = null!;
        public IEnumerable<BranchOfferDetalisResponseDto> Offers { get; set; } = [];
        public int OffersCount { get; private set; } = 0;


        //Cosntructor
        public BranchQueryResponseDto(DomainBranch domain)
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
