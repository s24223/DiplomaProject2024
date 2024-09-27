using Application.Database;
using Application.Database.Models;
using Domain.Entities.UserPart;
using Domain.Factories;

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
            await _context.UserProblems.AddAsync(databaseUserProblem, cancellation);
            await _context.SaveChangesAsync(cancellation);
            return databaseUserProblem.Id;
        }
    }
}
