using Application.VerticalSlice.OfferPart.DTOs.Create;

namespace Application.VerticalSlice.OfferPart.Services
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
