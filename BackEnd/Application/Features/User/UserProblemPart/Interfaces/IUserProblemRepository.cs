using Domain.Features.User.ValueObjects.Identificators;
using Domain.Features.UserProblem.Entities;
using Domain.Features.UserProblem.ValueObjects.Identificators;

namespace Application.Features.User.UserProblemPart.Interfaces
{
    public interface IUserProblemRepository
    {
        //==============================================================================================
        //DML
        Task<Guid> CreateUserProblemAndReturnIdAsync
            (
            DomainUserProblem userProblem,
            CancellationToken cancellation
            );

        Task SetNewStatusUserProblemForAuthorizedAsync
            (
            DomainUserProblem userProblem,
            CancellationToken cancellation
            );

        //==============================================================================================
        //DQL
        Task<DomainUserProblem> GetDomainUserProblemAsync
            (
            UserId userId,
            UserProblemId userProblemId,
            CancellationToken cancellation
            );
    }
}
