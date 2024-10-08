using Domain.Features.BranchOffer.Entities;

namespace Application.Features.Company.OfferBranchPart.Interfaces
{
    public interface IBranchOfferRepository
    {
        Task CreateBranchOfferAsync(DomainBranchOffer branchOffer, CancellationToken cancellation);
    }
}
