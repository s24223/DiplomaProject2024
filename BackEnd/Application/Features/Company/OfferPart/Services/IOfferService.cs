using Application.Features.Company.OfferPart.DTOs.Create;
using Application.Features.Company.OfferPart.DTOs.Update;

namespace Application.Features.Company.OfferPart.Services
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
