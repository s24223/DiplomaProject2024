using Application.Features.Companies.DTOs.CommandsBranchOffer.CommandsBranchOffer.CreateBranchOffer.Request;
using Application.Features.Companies.DTOs.CommandsBranchOffer.CommandsBranchOffer.CreateBranchOffer.Response;
using Application.Features.Companies.DTOs.CommandsBranchOffer.CommandsBranchOffer.UpdateBranchOffer.Request;
using Application.Features.Companies.DTOs.CommandsBranchOffer.CommandsBranchOffer.UpdateBranchOffer.Response;
using Application.Features.Companies.DTOs.CommandsBranchOffer.CommandsOffer.CreateOffer.Request;
using Application.Features.Companies.DTOs.CommandsBranchOffer.CommandsOffer.UpdateOffer.Request;
using Application.Shared.DTOs.Features.Companies.Responses;
using Application.Shared.DTOs.Response;
using System.Security.Claims;

namespace Application.Features.Companies.Services.CommandsBranchOffer
{
    public interface IBranchOfferCommandService
    {
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
        Task<ResponseItem<CreateBranchOffersResponseDto>> CreateBranchOffersAsync
           (
           IEnumerable<Claim> claims,
           IEnumerable<CreateBranchOfferRequestDto> dtos,
           CancellationToken cancellation
           );

        Task<ResponseItem<UpdateBranchOfferResponseDto>> UpdateBranchOffersAsync
            (
            IEnumerable<Claim> claims,
            IEnumerable<UpdateBranchOfferRequestDto> dtos,
            CancellationToken cancellation
            );
    }
}
