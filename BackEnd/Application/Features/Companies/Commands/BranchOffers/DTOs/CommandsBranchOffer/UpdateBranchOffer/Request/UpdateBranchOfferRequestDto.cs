﻿using Application.Shared.DTOs;
using System.ComponentModel.DataAnnotations;

namespace Application.Features.Companies.Commands.BranchOffers.DTOs.CommandsBranchOffer.UpdateBranchOffer.Request
{
    public class UpdateBranchOfferRequestDto
    {
        [Required]
        public required Guid BranchOfferId { get; set; }
        [Required]
        public required DateTime PublishStart { get; set; }
        public DateTime? PublishEnd { get; set; }
        public DateOnlyRequestDto? WorkStart { get; set; }
        public DateOnlyRequestDto? WorkEnd { get; set; }
    }
}
