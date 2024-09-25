using Application.Database;
using Domain.Entities.RecrutmentPart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.VerticalSlice.RecrutmentPart.Interfaces
{
    public class RecruitmentRepository : IRecruitmentRepository
    {

        private readonly DiplomaProjectContext _context;
        
        public RecruitmentRepository(DiplomaProjectContext context)
        {
            _context = context;
        }

        public async Task CreateRecruitmentAsync(DomainRecruitment recruitment, CancellationToken cancellation)
        {
            await _context.Recruitments.AddAsync(
                new Database.Models.Recruitment
                {
                    PersonId =recruitment.Id.PersonId.Value,
                    BranchId = recruitment.Id.BranchOfferId.BranchId.Value,
                    OfferId = recruitment.Id.BranchOfferId.OfferId.Value,
                    Created=recruitment.Id.BranchOfferId.Created,
                    ApplicationDate = recruitment.ApplicationDate,
                    PersonMessage= recruitment.PersonMessage,
                    CompanyResponse= recruitment.CompanyResponse,
                    AcceptedRejected=recruitment.AcceptedRejected.Code
                }
                );
            await _context.SaveChangesAsync();
            throw new NotImplementedException();
        }
    }
}
