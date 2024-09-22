using Application.Database;
using Application.Database.Models;
using Application.Shared.Exceptions.UserExceptions;
using Domain.Entities.UserPart;
using Domain.Factories;
using Microsoft.EntityFrameworkCore;

namespace Application.VerticalSlice.UserProblemPart.Interfaces
{
    public class UserProblemRepository : IUserProblemRepository
    {
        private readonly DiplomaProjectContext _context;
        private readonly IDomainFactory _domainFactory;

        public UserProblemRepository(
            DiplomaProjectContext diplomaProjectContext,
            IDomainFactory domainFactory
            )
        {
            _context = diplomaProjectContext;
            _domainFactory = domainFactory;
        }

        public async Task<Guid> CreateUserProblemAndReturnIdAsync
            (
            DomainUserProblem userProblem,
            CancellationToken cancellation
            )
        {
            if (userProblem.UserId != null)
            {
                var databaseUser = await _context.Users
                    .Where(x => x.Id == userProblem.UserId.Value)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(cancellation);
                if (databaseUser == null)
                {
                    throw new UnauthorizedUserException();
                }
            }
            var databaseUserProblem = new UserProblem
            {
                DateTime = userProblem.DateTime,
                UserMessage = userProblem.UserMessage,
                PreviousProblemId = (userProblem.PreviousProblemId == null) ?
                null : userProblem.PreviousProblemId.Value,
                UserId = (userProblem.UserId == null) ?
                null : userProblem.UserId.Value,
                Email = (userProblem.Email == null) ?
                null : userProblem.Email.Value,
                Status = userProblem.Status.Code,
            };
            await _context.UserProblems.AddAsync(databaseUserProblem);
            await _context.SaveChangesAsync();
            return databaseUserProblem.Id;
        }
    }
}
