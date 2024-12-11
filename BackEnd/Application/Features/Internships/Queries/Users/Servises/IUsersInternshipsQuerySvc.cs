using Application.Features.Internships.Queries.DTOs;
using Application.Shared.DTOs.Features.Internships.Comments;
using Application.Shared.DTOs.Response;
using System.Security.Claims;

namespace Application.Features.Internships.Queries.Users.Servises
{
    public interface IUsersInternshipsQuerySvc
    {
        Task<ResponseItem<InternshipWithCommentsResp>> CommentsFirstPageAsync(
            IEnumerable<Claim> claims,
            Guid internshipId,
            CancellationToken cancellation,
            string? searchText = null,
            int? commentType = null,
            DateTime? from = null,
            DateTime? to = null,
            string orderBy = "created", // CommentTypeId
            bool ascending = false,
            int maxItems = 100);

        Task<ResponseItems<CommentResp>> CommentsAsync(
            IEnumerable<Claim> claims,
            Guid internshipId,
            CancellationToken cancellation,
            string? searchText = null,
            int? commentType = null,
            DateTime? from = null,
            DateTime? to = null,
            string orderBy = "created", // CommentTypeId
            bool ascending = false,
            int maxItems = 100,
            int page = 1);
    }
}
