using Domain.Entities.UserPart;

namespace Application.VerticalSlice.UserProblemPart.Interfaces
{
    public interface IUserProblemRepository
    {
        /*Task CreateUserProblemAuthorizedAsync(
            IEnumerable<Claim> claims,
            string userMessage,
            CancellationToken cancellationToken
            );
        Task CreateUserProblemUnauthorizedAsync(
            string userMessage,
            Email email,
            CancellationToken cancelToken
            );*/
        Task<Guid> CreateUserProblemAndReturnIdAsync
            (
            DomainUserProblem userProblem,
            CancellationToken cancellation
            );
    }
}
