using Application.Database;
using Domain.Factories;
using System.Security.Claims;
using Application.Shared.Services.Authentication;
using Domain.ValueObjects.EntityIdentificators;
using Domain.ValueObjects;

namespace Application.VerticalSlice.UserPart.Interfaces
{
    public class UserProblem : IUserProblem
    {
        private readonly DiplomaProjectContext _context;
        private readonly IDomainFactory _domainFactory;
        private readonly IUserRepository _userRepository;
        private readonly IAuthenticationService _authentication;

        public UserProblem(
            DiplomaProjectContext diplomaProjectContext,
            IDomainFactory domainFactory,
            IUserRepository userRepository,
            IAuthenticationService authenticationService
            )
        {
            _context = diplomaProjectContext;
            _domainFactory = domainFactory;
            _userRepository = userRepository;
            _authentication = authenticationService;
        }

        public async Task CreateUserProblemAuthorizedAsync(IEnumerable<Claim> claims, string userMessage, CancellationToken cancellationToken)
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
        }
    }
}
