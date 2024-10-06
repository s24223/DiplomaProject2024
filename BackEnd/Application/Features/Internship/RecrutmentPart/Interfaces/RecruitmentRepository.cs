using Application.Database;
using Domain.Features.BranchOffer.Exceptions;
using Domain.Features.Recruitment.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Internship.RecrutmentPart.Interfaces
{
    public class RecruitmentRepository : IRecruitmentRepository
    {
        private readonly DiplomaProjectContext _context;

        public RecruitmentRepository(DiplomaProjectContext context)
        {
            _context = context;
        }

        public async Task CreateAsync
            (
            DomainRecruitment recruitment,
            CancellationToken cancellation
            )
        {
            var branchOffer = await _context.BranchOffers.Where(x =>
            x.OfferId == recruitment.Id.BranchOfferId.OfferId.Value &&
            x.BranchId == recruitment.Id.BranchOfferId.BranchId.Value &&
            x.Created == recruitment.Id.BranchOfferId.Created
            ).AsNoTracking().FirstOrDefaultAsync(cancellation);

            if (branchOffer == null)
            {
                throw new BranchOfferException("Messages.NotExistBranchOffer");
            }

            var databaseRecrutment = new Database.Models.Recruitment
            {
                PersonId = recruitment.Id.PersonId.Value,
                BranchId = recruitment.Id.BranchOfferId.BranchId.Value,
                OfferId = recruitment.Id.BranchOfferId.OfferId.Value,
                Created = recruitment.Id.BranchOfferId.Created,
                ApplicationDate = recruitment.ApplicationDate,
                PersonMessage = recruitment.PersonMessage,
            };

            await _context.Recruitments.AddAsync(databaseRecrutment, cancellation);
            await _context.SaveChangesAsync(cancellation);
        }
    }
}
