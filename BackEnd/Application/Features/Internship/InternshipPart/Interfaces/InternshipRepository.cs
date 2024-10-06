using Application.Database;
using Domain.Features.Intership.Entities;

namespace Application.Features.Internship.InternshipPart.Interfaces
{
    public class InternshipRepository : IInternshipRepository
    {
        private readonly DiplomaProjectContext _context;

        public InternshipRepository(DiplomaProjectContext context)
        {
            _context = context;
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
            await _context.SaveChangesAsync();
        }
    }
}
