﻿using Application.Features.Companies.DTOs.QueriesCompany.Shared;
using Application.Shared.DTOs.Features.Companies;
using Domain.Features.BranchOffer.Entities;

namespace Application.Features.Companies.DTOs.QueriesCompany.QueriesOffer
{
    public class BranchOfferQueryResponseDto
    {
        //Values
        public CompanyDetailsResponseDto Company { get; set; } = null!;
        public BranchResponseDto Branch { get; set; } = null!;
        public OfferResponseDto Offer { get; set; } = null!;
        public BranchOfferResponseDto OfferDetails { get; set; } = null!;


        //Cosntructor
        public BranchOfferQueryResponseDto(DomainBranchOffer domain)
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
