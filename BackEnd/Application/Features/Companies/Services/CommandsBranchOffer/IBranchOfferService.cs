using Application.Features.Companies.DTOs.CommandsBranchOffer.CommandsBranchOffer.CreateBranchOffer;
using Application.Features.Companies.DTOs.CommandsBranchOffer.CommandsBranchOffer.UpdateBranchOffer;
using Application.Features.Companies.DTOs.CommandsBranchOffer.CommandsOffer.CreateOffer;
using Application.Features.Companies.DTOs.CommandsBranchOffer.CommandsOffer.UpdateOffer;
using Application.Shared.DTOs.Features.Companies;
using Application.Shared.DTOs.Response;
using System.Security.Claims;

namespace Application.Features.Companies.Services.CommandsBranchOffer
{
    public interface IBranchOfferService
    {
        //DML

        //Offer Part
        Task<ResponseItems<OfferResponseDto>> CreateOffersAsync
           (
           IEnumerable<Claim> claims,
           IEnumerable<CreateOfferRequestDto> dtos,
           CancellationToken cancellation
           );

        Task<ResponseItems<OfferResponseDto>> UpdateOffersAsync
            (
            IEnumerable<Claim> claims,
            IEnumerable<UpdateOfferRequestDto> dtos,
            CancellationToken cancellation
            );

        //BranchOffer Part
        Task<ResponseItems<CreateBranchOfferResponseDto>> CreateBranchOfferAsync
           (
           IEnumerable<Claim> claims,
           IEnumerable<CreateBranchOfferRequestDto> dtos,
           CancellationToken cancellation
           );

        Task<ResponseItems<BranchOfferResponseDto>> UpdateBranchOfferAsync
            (
            IEnumerable<Claim> claims,
            IEnumerable<UpdateBranchOfferRequestDto> dtos,
            CancellationToken cancellation
            );
    }
}
