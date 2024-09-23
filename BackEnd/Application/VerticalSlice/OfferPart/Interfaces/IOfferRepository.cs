using Domain.Entities.CompanyPart;


namespace Application.VerticalSlice.OfferPart.Interfaces
{
    public interface IOfferRepository
    {
        Task CreateOfferProfileAsync(DomainOffer offer, CancellationToken cancellation);
    }
}
