using Domain.Features.User.ValueObjects.Identificators;
using Domain.Features.UserProblem.Entities;
using Domain.Features.UserProblem.ValueObjects.Identificators;

namespace Application.Features.User.UserProblemPart.Interfaces
{
    public interface IUserProblemRepository
    {
        //==============================================================================================
        //DML
        Task<Guid> CreateAndReturnIdAsync
            (
            DomainUserProblem userProblem,
            CancellationToken cancellation
            );

        Task SetNewStatusForAuthorizedAsync
            (
            UserId userId,
            DomainUserProblem userProblem,
            CancellationToken cancellation
            );

        //==============================================================================================
        //DQL
        Task<DomainUserProblem> GetProblemAsync
            (
            UserId userId,
            UserProblemId userProblemId,
            CancellationToken cancellation
            );
    }
}
