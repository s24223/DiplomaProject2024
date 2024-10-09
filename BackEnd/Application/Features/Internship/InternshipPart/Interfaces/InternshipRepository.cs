using Application.Database;
using Application.Features.Internship.InternshipPart.DTOs;
using Domain.Features.Intership.Entities;
using Domain.Shared.Factories;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Internship.InternshipPart.Interfaces
{
    public class InternshipRepository : IInternshipRepository
    {
        private readonly DiplomaProjectContext _context;
        private readonly IDomainFactory _domainFactory;

        public InternshipRepository(DiplomaProjectContext context,
            IDomainFactory domainFactory)
        {
            _context = context;
            _domainFactory = domainFactory;
        }
        public async Task CreateInternshipAsync(DomainIntership intership, CancellationToken cancellaction)
        {
            var internshipDb = new Application.Database.Models.Internship
            {
                Id = intership.Id.Value,
                ContractNumber = intership.ContractNumber,
                PersonId = intership.RecrutmentId.PersonId.Value,
                BranchId = intership.RecrutmentId.BranchOfferId.BranchId.Value,
                OfferId = intership.RecrutmentId.BranchOfferId.OfferId.Value,
                Created = intership.RecrutmentId.BranchOfferId.Created,
            };
            await _context.Internships.AddAsync(
                internshipDb, cancellaction);
            await _context.SaveChangesAsync(cancellaction);
        }

        public async Task<DomainIntership> GetInternshipAsync(Guid id, CancellationToken cancellation)
        {
            var intership = await _context.Internships.Where(x => x.Id == id)
                .FirstOrDefaultAsync(cancellation);
            if (intership == null)
                throw new Exception();
            var domainInternship = _domainFactory.CreateDomainInternship(
                intership.ContractNumber,
                intership.PersonId,
                intership.BranchId,
                intership.OfferId,
                intership.Created);
            return domainInternship;
        }

        public async Task UpdateInternshipAsync(DomainIntership intership, CancellationToken cancellaction)
        {
            var internshipDb = await _context.Internships
                .Where(x => x.Id == intership.Id.Value)
                .FirstOrDefaultAsync(cancellaction);

            if (internshipDb == null)
                throw new Exception();

            internshipDb.ContractNumber = intership.ContractNumber;

            await _context.SaveChangesAsync(cancellaction);
        }
    }
}
