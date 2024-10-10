using Application.Features.Companies.OfferPart.DTOs.Create;
using Application.Features.Companies.OfferPart.DTOs.Update;
using Application.Shared.DTOs.Response;
using System.Security.Claims;

namespace Application.Features.Companies.OfferPart.Services
{
    public interface IOfferService
    {
        //DML
        Task<ResponseItem<CreateOfferResponseDto>> CreateAsync
           (
           IEnumerable<Claim> claims,
           CreateOfferRequestDto dto,
           CancellationToken cancellation
           );

        Task<Response> UpdateAsync
            (
            IEnumerable<Claim> claims,
            Guid offerId,
            UpdateOfferRequestDto dto,
            CancellationToken cancellation
            );
        //DQL
    }
}
