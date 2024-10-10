using Application.Features.Companies.BranchOfferPart.DTOs.CreateBranchOffer;
using Application.Features.Companies.BranchOfferPart.DTOs.CreateOffer;
using Application.Features.Companies.BranchOfferPart.DTOs.UpdateBranchOffer;
using Application.Features.Companies.BranchOfferPart.DTOs.UpdateOffer;
using Application.Shared.DTOs.Response;
using System.Security.Claims;

namespace Application.Features.Companies.BranchOfferPart.Services
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
            Guid branchId,
            Guid offerId,
            DateTime created,
            UpdateBranchOfferRequestDto dto,
            CancellationToken cancellation
            );
        //DQL
    }
}
