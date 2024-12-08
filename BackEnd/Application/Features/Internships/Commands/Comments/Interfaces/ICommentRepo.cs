using Domain.Features.Comment.Entities;
using Domain.Features.Comment.ValueObjects.CommentTypePart;
using Domain.Features.Comment.ValueObjects.Identificators;
using Domain.Features.User.ValueObjects.Identificators;

namespace Application.Features.Internships.Commands.Comments.Interfaces
{
    public interface ICommentRepo
    {
        Task<CommentSenderEnum> GetSenderRoleAsync(
            Guid intershipId,
            UserId userId,
            CancellationToken cancellation);

        Task<DomainComment> CreateCommentAsync(
            DomainComment domain,
            CancellationToken cancellation);

        Task DeleteAsync(
            CommentId id,
            CancellationToken cancellation);

    }
}
