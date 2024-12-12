using Application.Shared.DTOs.Features.Internships;
using Domain.Features.BranchOffer.Entities;
using Domain.Features.BranchOffer.ValueObjects.Identificators;
using Domain.Features.Comment.Entities;
using Domain.Features.Intership.Entities;
using Domain.Features.Recruitment.Entities;
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

        Task<(IEnumerable<DomainComment> Items, int TotalCount)> GetCommentsAsync(
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

        Task<(IEnumerable<(DomainIntership Intership, InternshipDetailsResp Details)> Items,
                int TotalCount)> GetInternshipsForPersonAsync(
           UserId personId,
           CancellationToken cancellation,
           string? searchText = null,
           DateTime? from = null,
           DateTime? to = null,
           string orderBy = "created", // ContractStartDate
           bool ascending = true,
           int maxItems = 100,
           int page = 1);

        Task<(IEnumerable<(DomainIntership Intership, InternshipDetailsResp Details)> Items,
                int TotalCount)> GetInternshipsForCompanyAsync(
           UserId companyId,
           CancellationToken cancellation,
           string? searchText = null,
           DateTime? from = null,
           DateTime? to = null,
           string orderBy = "created", // ContractStartDate
           bool ascending = true,
           int maxItems = 100,
           int page = 1);


        Task<(IEnumerable<DomainRecruitment> Items, int TotalCount)>
            GetPersonRecruitmentsAsync(
            UserId personId,
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

        Task<(IEnumerable<DomainRecruitment> Items, int TotalCount)>
            GetCompanyRecruitmentsAsync(
            UserId companyId,
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

        Task<(DomainBranchOffer BranchOffer, int TotalCount)>
            GetBranchOfferRecruitmentsFirstPageAsync(
            UserId companyId,
            BranchOfferId branchOfferId,
            CancellationToken cancellation,
            string? searchText = null,
            DateTime? from = null,
            DateTime? to = null,
            bool filterStatus = false,
            bool? status = null, // true accepted, false denied
            string orderBy = "created", // ContractStartDate
            bool ascending = true,
            int maxItems = 100);

        Task<(IEnumerable<DomainRecruitment> Items, int TotalCount)>
            GetBranchOfferRecruitmentsAsync(
            UserId companyId,
            BranchOfferId branchOfferId,
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
