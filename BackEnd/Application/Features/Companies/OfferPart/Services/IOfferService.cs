using Application.Features.Companies.OfferPart.DTOs.Create;
using Application.Features.Companies.OfferPart.DTOs.Update;

namespace Application.Features.Companies.OfferPart.Services
{
    public interface IOfferService
    {
        Task CreateOfferProfileAsync
             (
             CreateOfferRequestDto dto,
             CancellationToken cancellation
             );
        Task UpdateOfferProfileAsync
            (

            Guid id,
            UpdateOfferRequestDto dto,
            CancellationToken cancellation
            );
    }
}
