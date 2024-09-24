using Domain.Entities.CompanyPart;

namespace Application.VerticalSlice.OfferBranchPart.Interfaces
{
    public interface IBranchOfferRepository
    {
        Task CreateBranchOfferAsync(DomainBranchOffer branchOffer, CancellationToken cancellation);
    }
}
