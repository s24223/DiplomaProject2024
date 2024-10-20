using Application.Features.Companies.DTOs.CommandsBranchOffer.CreateBranchOffer;
using Application.Features.Companies.DTOs.CommandsBranchOffer.CreateOffer;
using Application.Features.Companies.DTOs.CommandsBranchOffer.UpdateBranchOffer;
using Application.Features.Companies.DTOs.CommandsBranchOffer.UpdateOffer;
using Application.Shared.DTOs.Response;
using System.Security.Claims;

namespace Application.Features.Companies.Services.CommandsBranchOffer
{
    public interface IBranchOfferService
    {
        //DML

        //Offer Part
        Task<ResponseItem<CreateOfferResponseDto>> CreateOfferAsync
           (
           IEnumerable<Claim> claims,
           CreateOfferRequestDto dto,
           CancellationToken cancellation
           );

        Task<Response> UpdateOfferAsync
            (
            IEnumerable<Claim> claims,
            Guid offerId,
            UpdateOfferRequestDto dto,
            CancellationToken cancellation
            );

        //BranchOffer Part
        Task<Response> CreateBranchOfferConnectionAsync
           (
           IEnumerable<Claim> claims,
           Guid branchId,
           Guid offerId,
           CreateBranchOfferRequestDto dto,
           CancellationToken cancellation
           );

        Task<Response> UpdateBranchOfferConnectionAsync
            (
            IEnumerable<Claim> claims,
            Guid branchofferId,
            UpdateBranchOfferRequestDto dto,
            CancellationToken cancellation
            );
        //DQL

        //Offer Part
        //BranchOffer Part

    }
}
