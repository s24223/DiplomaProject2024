using Application.Database;
using Domain.Features.Branch.Entities;
using Domain.Features.Url.Exceptions.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Company.BranchPart.Interfaces
{
    public class BranchRepository : IBranchRepository
    {
        private readonly DiplomaProjectContext _context;
        public BranchRepository(DiplomaProjectContext context)
        {
            _context = context;
        }
        public async Task CreateBranchProfileAsync
            (
            DomainBranch branch,
            CancellationToken cancellation
            )
        {
            await _context.Branches.AddAsync
                (
                new Database.Models.Branch
                {
                    CompanyId = branch.CompanyId.Value,
                    AddressId = branch.AddressId.Value,
                    Id = branch.Id.Value,
                    UrlSegment = branch.UrlSegment.Value,
                    Name = branch.Name,
                    Description = branch.Description

                });
            await _context.SaveChangesAsync();
        }









        /// <summary>
        /// TU TRZEBA POPRAWIĆ!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

        public async Task UpdateBranchProfileAsync
            (

            DomainBranch branch,
            CancellationToken cancellation
            )
        {
            try
            {
                var databaseBranch = await _context.Branches
                    .Where(x => x.Id == branch.Id.Value /*||
                        x.CompanyId==branch.CompanyId.Value ||
                        x.AddressId==branch.AddressId.Value*/
                    )
                    .FirstOrDefaultAsync(
                    cancellation);
                if (databaseBranch == null)
                {
                    throw new UrlException(Messages.NotFoundUrl);
                }
            }
            catch (System.Exception ex)
            {

            }
        }
    }
}
