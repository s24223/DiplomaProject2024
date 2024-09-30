using Application.Database;
using Application.Database.Models;
using Domain.Entities.RecrutmentPart;

namespace Application.VerticalSlice.InternshipPart.Interfaces
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
            var internshipDb = new Internship
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
