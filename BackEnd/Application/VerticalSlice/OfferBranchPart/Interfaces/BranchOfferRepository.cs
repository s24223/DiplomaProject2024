using Application.Database;
using Domain.VerticalSlice.BranchOfferPart.Entities;

namespace Application.VerticalSlice.OfferBranchPart.Interfaces
{
    public class OfferBranchRepository : IBranchOfferRepository
    {
        private readonly DiplomaProjectContext _context;

        public OfferBranchRepository(DiplomaProjectContext context)
        {
            _context = context;
        }

        public async Task CreateBranchOfferAsync(DomainBranchOffer branchOffer, CancellationToken cancellation)
        {
            await _context.BranchOffers.AddAsync(
                new Database.Models.BranchOffer
                {
                    BranchId = branchOffer.Branch.Id.Value,
                    OfferId = branchOffer.Offer.Id.Value,
                    Created = branchOffer.Id.Created,
                    PublishStart = branchOffer.PublishStart,
                    PublishEnd = branchOffer.PublishEnd,
                    WorkStart = branchOffer.WorkStart,
                    WorkEnd = branchOffer.WorkEnd,
                    LastUpdate = branchOffer.LastUpdate,
                });
            await _context.SaveChangesAsync();
        }
    }
}
