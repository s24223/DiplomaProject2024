using Application.Features.Internships.Queries.DTOs.Comments;
using Application.Features.Internships.Queries.DTOs.Internships;
using Application.Features.Internships.Queries.DTOs.Recritments;
using Application.Features.Internships.Queries.DTOs.Recritments.BranchOffers;
using Application.Shared.DTOs.Features.Internships.Comments;
using Application.Shared.DTOs.Response;
using System.Security.Claims;

namespace Application.Features.Internships.Queries.Users.Servises
{
    public interface IUsersInternshipsQuerySvc
    {
        Task<ResponseItem<CommentsWithInternshipResp>> CommentsFirstPageAsync(
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

        Task<ResponseItems<CompanyInternshipResp>> GetInternshipsForCompanyAsync(
           IEnumerable<Claim> claims,
           CancellationToken cancellation,
           string? searchText = null,
           DateTime? from = null,
           DateTime? to = null,
           string orderBy = "created", // ContractStartDate
           bool ascending = true,
           int maxItems = 100,
           int page = 1);

        Task<ResponseItems<PersonInternshipResp>> GetInternshipsForPersonAsync(
           IEnumerable<Claim> claims,
           CancellationToken cancellation,
           string? searchText = null,
           DateTime? from = null,
           DateTime? to = null,
           string orderBy = "created", // ContractStartDate
           bool ascending = true,
           int maxItems = 100,
           int page = 1);

        Task<ResponseItems<CompanyRecruitmentResp>> GetCompanyRecruitmentsAsync(
            IEnumerable<Claim> claims,
            CancellationToken cancellation,
            string? searchText = null,
            DateTime? from = null,
            DateTime? to = null,
            bool filterStatus = false,
            bool? status = null, // true accepted, false denied
            string orderBy = "created", // ContractStartDate
            bool ascending = true,
            int maxItems = 100,
            int page = 1);

        Task<ResponseItems<PersonRecruitmentResp>> GetPersonRecruitmentsAsync(
            IEnumerable<Claim> claims,
            CancellationToken cancellation,
            string? searchText = null,
            DateTime? from = null,
            DateTime? to = null,
            bool filterStatus = false,
            bool? status = null, // true accepted, false denied
            string orderBy = "created", // ContractStartDate
            bool ascending = true,
            int maxItems = 100,
            int page = 1);

        Task<ResponseItem<BranchOfferRecruitmentResp>> GetBranchOfferRecruitmentsFirstPageAsync(
           IEnumerable<Claim> claims,
           Guid branchOfferId,
           CancellationToken cancellation,
           string? searchText = null,
           DateTime? from = null,
           DateTime? to = null,
           bool filterStatus = false,
           bool? status = null, // true accepted, false denied
           string orderBy = "created", // ContractStartDate
           bool ascending = true,
           int maxItems = 100);

        Task<ResponseItems<BranchOfferPersonRecruitmentResp>> GetBranchOfferRecruitmentsAsync(
            IEnumerable<Claim> claims,
            Guid branchOfferId,
            CancellationToken cancellation,
            string? searchText = null,
            DateTime? from = null,
            DateTime? to = null,
            bool filterStatus = false,
            bool? status = null, // true accepted, false denied
            string orderBy = "created", // ContractStartDate
            bool ascending = true,
            int maxItems = 100,
            int page = 1);
    }
}
