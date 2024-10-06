using Domain.Features.Offer.Entities;

namespace Application.Features.Company.OfferPart.Interfaces
{
    public interface IOfferRepository
    {
        Task CreateOfferProfileAsync(DomainOffer offer, CancellationToken cancellation);
    }
}
