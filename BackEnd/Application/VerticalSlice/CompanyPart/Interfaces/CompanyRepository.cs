using Application.Database;
using Application.Shared.Exceptions.UserExceptions;
using Domain.Entities.CompanyPart;
using Microsoft.EntityFrameworkCore;

namespace Application.VerticalSlice.CompanyPart.Interfaces
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly DiplomaProjectContext _context;

        public CompanyRepository(DiplomaProjectContext context)
        {
            _context = context;
        }

        public async Task CreateCompanyProfileAsync(DomainCompany company, CancellationToken cancellation)
        {
            var databaseCompany = await _context.Companies
                .Where(x => x.UserId == company.Id.Value)
                .FirstOrDefaultAsync(cancellation);
            if (databaseCompany != null)
            {
                //exeption
            }
            var databaseUser = await _context.Users
                .Where(x => x.Id == company.Id.Value)
                .FirstOrDefaultAsync(cancellation);

            if (databaseUser == null)
            {
                throw new UnauthorizedUserException();
            }

            //Uerl segment, Emaiil Unique 
            await _context.Companies.AddAsync(new Database.Models.Company
            {
                User = databaseUser,
                UrlSegment = (company.UrlSegment == null) ? null : company.UrlSegment.Value,
                CreateDate = company.CreateDate,
                ContactEmail = company.ContactEmail.Value,
                Name = company.Name,
                Regon = company.Regon.Value,
            }, cancellation);
            await _context.SaveChangesAsync(cancellation);
        }
    }
}
