using Domain.Entities.UserPart;

namespace Application.VerticalSlice.UserProblemPart.Interfaces
{
    public interface IUserProblemRepository
    {
        Task<Guid> CreateUserProblemAndReturnIdAsync
            (
            DomainUserProblem userProblem,
            CancellationToken cancellation
            );
    }
}
