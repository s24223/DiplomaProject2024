using Application.Database;
using Domain.Features.BranchOffer.Exceptions.AppExceptions;
using Domain.Features.Recruitment.Entities;
using Domain.Shared.Factories;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Internship.RecrutmentPart.Interfaces
{
    public class RecruitmentRepository : IRecruitmentRepository
    {
        private readonly DiplomaProjectContext _context;
        private readonly IDomainFactory _domainFactory;

        public RecruitmentRepository(DiplomaProjectContext context,
            IDomainFactory domain)
        {
            _context = context;
            _domainFactory = domain;
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

        public async Task<DomainRecruitment> GetRecruitmentAsync(Guid id, CancellationToken cancellation)
        {
            var recruitment = await _context.Recruitments
                .Where(x => x.PersonId == id)
                .FirstOrDefaultAsync(cancellation);
            if (recruitment == null)
                throw new Exception();
            var recruitmentDomain = _domainFactory.CreateDomainRecruitment(
                recruitment.PersonId,
                recruitment.BranchId,
                recruitment.OfferId,
                recruitment.Created,
                recruitment.ApplicationDate,
                recruitment.PersonMessage,
                recruitment.CompanyResponse,
                recruitment.IsAccepted);
            return recruitmentDomain;
        }

        public async Task UpdateRecruitmentAsync(DomainRecruitment recruitment, CancellationToken cancellation)
        {
            var recruitmentDb = await _context.Recruitments
                .Where(x => x.PersonId == recruitment.Person.Id.Value)
                .FirstOrDefaultAsync(cancellation);
            if (recruitmentDb == null)
                throw new Exception();
            recruitmentDb.CompanyResponse = recruitment.CompanyResponse;
            if (recruitment.AcceptedRejected == null
                || !recruitment.AcceptedRejected.Value)
                recruitmentDb.IsAccepted = "F";
            else
                recruitmentDb.IsAccepted = "T";
            await _context.SaveChangesAsync(cancellation);
        }
    }
}
