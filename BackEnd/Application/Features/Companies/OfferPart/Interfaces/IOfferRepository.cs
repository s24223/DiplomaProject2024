using Domain.Features.Offer.Entities;

namespace Application.Features.Companies.OfferPart.Interfaces
{
    public interface IOfferRepository
    {
        Task CreateOfferProfileAsync(DomainOffer offer, CancellationToken cancellation);
        Task UpdateOfferProfileAsync
            (
                DomainOffer offer,
                CancellationToken cancellation
            );
    }
}
