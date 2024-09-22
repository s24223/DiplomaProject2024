using Application.Database;
using Application.Database.Models;
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

        /*public async Task CreateUserProblemAuthorizedAsync(IEnumerable<Claim> claims, string userMessage, CancellationToken cancellationToken)
        {
            var id = _authentication.GetIdNameFromClaims(claims);
            var tuple = await _userRepository.GetUserDataByIdAsync(new UserId(id), cancellationToken);
            var mail = tuple.User.Login;
            await _context.UserProblems.AddAsync(new Database.Models.UserProblem
            {
                DateTime = DateTime.Now,
                UserMessage = userMessage,
                UserId = id,
                Email = mail.Value,
                Status = "C"
            });
            await _context.SaveChangesAsync();
        }

        public async Task CreateUserProblemUnauthorizedAsync(string userMessage, Email email, CancellationToken cancelToken)
        {
            await _context.UserProblems.AddAsync(new Database.Models.UserProblem
            {
                DateTime = DateTime.Now,
                UserMessage = userMessage,
                Email = email.Value,
                Status = "C"
            });
            await _context.SaveChangesAsync();
        }*/

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
                    throw new UnauthorizedAccessException();
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
