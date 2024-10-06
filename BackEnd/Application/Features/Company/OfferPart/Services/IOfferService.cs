using Application.Features.Company.OfferPart.DTOs.Create;

namespace Application.Features.Company.OfferPart.Services
{
    public interface IOfferService
    {
        Task CreateOfferProfileAsync
             (
             CreateOfferRequestDto dto,
             CancellationToken cancellation
             );
    }
}
