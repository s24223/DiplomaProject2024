using Domain.Entities.UserPart;
using Domain.ValueObjects.EntityIdentificators;

namespace Application.VerticalSlice.UserProblemPart.Interfaces
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
