using Domain.Features.BranchOffer.Entities;

namespace Application.Features.Companies.BranchOfferPart.Interfaces
{
    public interface IBranchOfferRepository
    {
        Task CreateBranchOfferAsync(DomainBranchOffer branchOffer, CancellationToken cancellation);
    }
}
