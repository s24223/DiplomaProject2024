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
        Task<Guid> CreateOfferAsync
            (
            UserId companyId,
            DomainOffer offer,
            CancellationToken cancellation
            );

        Task UpdateOfferAsync
            (
            UserId companyId,
            DomainOffer offer,
            CancellationToken cancellation
            );

        //BranchOffer Part
        Task CreateBranchOfferAsync
            (
            UserId companyId,
            DomainBranchOffer branchOffer,
            CancellationToken cancellation
            );

        Task UpdateBranchOfferAsync
            (
            UserId companyId,
            DomainBranchOffer branchOffer,
            CancellationToken cancellation
            );

        //DQL
        Task<DomainOffer> GetOfferAsync
            (
            UserId companyId,
            OfferId id,
            CancellationToken cancellation
            );

        Task<DomainBranchOffer> GetBranchOfferAsync
            (
            UserId companyId,
            BranchOfferId id,
            CancellationToken cancellation
            );
    }
}
