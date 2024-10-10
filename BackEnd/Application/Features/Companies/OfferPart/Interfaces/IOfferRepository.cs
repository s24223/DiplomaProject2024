using Domain.Features.Offer.Entities;
using Domain.Features.Offer.ValueObjects.Identificators;
using Domain.Features.User.ValueObjects.Identificators;

namespace Application.Features.Companies.OfferPart.Interfaces
{
    public interface IOfferRepository
    {
        //DML
        Task<Guid> CreateAsync
            (
            UserId companyId,
            DomainOffer offer,
            CancellationToken cancellation
            );

        Task UpdateAsync
            (
            UserId companyId,
            DomainOffer offer,
            CancellationToken cancellation
            );

        //DQL
        Task<DomainOffer> GetOfferAsync
            (
            UserId companyId,
            OfferId id,
            CancellationToken cancellation
            );
    }
}
