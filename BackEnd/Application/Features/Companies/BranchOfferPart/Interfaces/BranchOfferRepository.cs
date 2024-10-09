using Application.Database;
using Application.Database.Models;
using Domain.Features.BranchOffer.Entities;
using Domain.Features.Url.Entities;

namespace Application.Features.Companies.BranchOfferPart.Interfaces
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
                new BranchOffer
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


        /*public async Task UpdateAsync
            (
           //Guid id,
            DomainBranchOffer branchOffer,
            CancellationToken cancellation
            )
        {
            try
            {
                var databaseOffer = await _context.Offers
                   .Where(x => x.Creted==)
                   .FirstOrDefaultAsync(
                   cancellation);
            }
            catch (System.Exception ex)
            {

            }*/
    }

}
