﻿using Application.Shared.DTOs.Features.Companies;
using Domain.Features.BranchOffer.Entities;

namespace Application.Features.Companies.DTOs.CommandsBranchOffer.CommandsBranchOffer.CreateBranchOffer
{
    public class CreateBranchOfferResponseDto
    {
        //values
        public OfferResponseDto Offer { get; set; } = null!;
        public BranchOfferResponseDto OfferDetails { get; set; } = null!;


        //Cosntructor
        public CreateBranchOfferResponseDto(DomainBranchOffer domain)
        {
            OfferDetails = new BranchOfferResponseDto(domain);
            if (domain.Offer != null)
            {
                Offer = new OfferResponseDto(domain.Offer);
            }
        }
    }
}
