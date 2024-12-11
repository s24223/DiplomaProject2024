using Application.Shared.DTOs.Features.Internships;
using Domain.Features.Comment.Entities;
using Domain.Features.Intership.Entities;
using Domain.Features.Recruitment.ValueObjects.Identificators;
using Domain.Features.User.ValueObjects.Identificators;

namespace Application.Features.Internships.Queries.Users.Interfaces
{
    public interface IUsersInternshipsRepo
    {
        Task<(InternshipDetailsResp Details, DomainIntership Intership, int TotalCount)>
            GetCommentsFirstPageAsync(
            UserId userId,
            RecrutmentId internshipId,
            CancellationToken cancellation,
            string? searchText = null,
            int? commentType = null,
            DateTime? from = null,
            DateTime? to = null,
            string orderBy = "created", // CommentTypeId
            bool ascending = false,
            int maxItems = 100);

        Task<IEnumerable<DomainComment>> GetCommentsAsync(
           UserId userId,
           RecrutmentId internshipId,
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
