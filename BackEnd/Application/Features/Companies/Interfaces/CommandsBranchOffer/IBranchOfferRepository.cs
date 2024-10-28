using Domain.Features.BranchOffer.Entities;
using Domain.Features.BranchOffer.ValueObjects.Identificators;
using Domain.Features.Offer.Entities;
using Domain.Features.Offer.ValueObjects.Identificators;
using Domain.Features.User.ValueObjects.Identificators;

namespace Application.Features.Companies.Interfaces.CommandsBranchOffer
{
    public interface IBranchOfferRepository
    {
        //DML
        //Offer Part
        Task<IEnumerable<DomainOffer>> CreateOffersAsync
            (
            UserId companyId,
            IEnumerable<DomainOffer> offers,
            CancellationToken cancellation
            );

        Task UpdateOffersAsync
            (
            UserId companyId,
            Dictionary<OfferId, DomainOffer> offers,
            CancellationToken cancellation
            );

        //DQL
        Task<Dictionary<OfferId, DomainOffer>> GetOfferDictionaryAsync
            (
            UserId companyId,
            IEnumerable<OfferId> ids,
            CancellationToken cancellation
            );


        //DML
        //BranchOffer Part
        Task<IEnumerable<DomainBranchOffer>> CreateBranchOffersAsync
            (
            UserId companyId,
            IEnumerable<DomainBranchOffer> branchOffers,
            CancellationToken cancellation
            );

        Task UpdateBranchOfferAsync
            (
            UserId companyId,
            Dictionary<BranchOfferId, DomainBranchOffer> dictionary,
            CancellationToken cancellation
            );

        //DQL
        Task<Dictionary<BranchOfferId, DomainBranchOffer>> GetBranchOfferDictionaryAsync
            (
            UserId companyId,
            IEnumerable<BranchOfferId> ids,
            CancellationToken cancellation
            );
    }
}
