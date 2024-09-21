using Domain.ValueObjects;
using System.Security.Claims;

namespace Application.VerticalSlice.UserPart.Interfaces
{
    public interface IUserProblem
    {
        Task CreateUserProblemAuthorizedAsync(
            IEnumerable<Claim> claims,
            string userMessage,
            CancellationToken cancellationToken
            );
        Task CreateUserProblemUnauthorizedAsync(
            string userMessage,
            Email email,
            CancellationToken cancelToken
            );
    }
}
