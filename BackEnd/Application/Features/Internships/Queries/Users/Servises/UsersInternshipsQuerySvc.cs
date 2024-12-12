using Application.Features.Internships.Queries.DTOs.Comments;
using Application.Features.Internships.Queries.DTOs.Internships;
using Application.Features.Internships.Queries.DTOs.Recritments;
using Application.Features.Internships.Queries.Users.Interfaces;
using Application.Shared.DTOs.Features.Internships.Comments;
using Application.Shared.DTOs.Response;
using Application.Shared.Services.Authentication;
using Domain.Features.Recruitment.ValueObjects.Identificators;
using Domain.Features.User.ValueObjects.Identificators;
using System.Security.Claims;

namespace Application.Features.Internships.Queries.Users.Servises
{
    public class UsersInternshipsQuerySvc : IUsersInternshipsQuerySvc
    {
        //Values
        private readonly IUsersInternshipsRepo _repo;
        private readonly IAuthJwtSvc _jwt;


        //Constructor
        public UsersInternshipsQuerySvc(
            IUsersInternshipsRepo repo,
            IAuthJwtSvc jwt)
        {
            _repo = repo;
            _jwt = jwt;
        }


        //===============================================================================================
        //===============================================================================================
        //===============================================================================================
        //Public Methods
        public async Task<ResponseItem<CommentsWithInternshipResp>> CommentsFirstPageAsync(
            IEnumerable<Claim> claims,
            Guid internshipId,
            CancellationToken cancellation,
            string? searchText = null,
            int? commentType = null,
            DateTime? from = null,
            DateTime? to = null,
            string orderBy = "created", // CommentTypeId
            bool ascending = false,
            int maxItems = 100)
        {
            var id = GetUserId(claims);
            var result = await _repo.GetCommentsFirstPageAsync(
                id,
                new RecrutmentId(internshipId),
                cancellation,
                searchText,
                commentType,
                from,
                to,
                orderBy,
                ascending,
                maxItems);

            return new ResponseItem<CommentsWithInternshipResp>
            {
                Item = new CommentsWithInternshipResp(
                    result.Intership,
                    result.Details,
                    result.TotalCount),
            };
        }


        public async Task<ResponseItems<CommentResp>> CommentsAsync(
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
            int page = 1)
        {
            var id = GetUserId(claims);

            var result = await _repo.GetCommentsAsync(
                id,
                new RecrutmentId(internshipId),
                cancellation,
                searchText,
                commentType,
                from,
                to,
                orderBy,
                ascending,
                maxItems,
                page);

            return new ResponseItems<CommentResp>
            {
                Items = result.Select(x => new CommentResp(x)).ToList(),
            };
        }

        public async Task<ResponseItems<CompanyInternshipResp>> GetInternshipsForCompanyAsync(
           IEnumerable<Claim> claims,
           CancellationToken cancellation,
           string? searchText = null,
           DateTime? from = null,
           DateTime? to = null,
           string orderBy = "created", // ContractStartDate
           bool ascending = true,
           int maxItems = 100,
           int page = 1)
        {
            var userId = GetUserId(claims);
            var result = await _repo.GetInternshipsForCompanyAsync(
                userId,
                cancellation,
                searchText,
                from,
                to,
                orderBy,
                ascending,
                maxItems,
                page
                );
            return new ResponseItems<CompanyInternshipResp>
            {
                Items = result.Items.Select(x =>
                    new CompanyInternshipResp(x.Intership, x.Details)).ToList(),
                TotalCount = result.TotalCount,
            };
        }

        public async Task<ResponseItems<PersonInternshipResp>> GetInternshipsForPersonAsync(
           IEnumerable<Claim> claims,
           CancellationToken cancellation,
           string? searchText = null,
           DateTime? from = null,
           DateTime? to = null,
           string orderBy = "created", // ContractStartDate
           bool ascending = true,
           int maxItems = 100,
           int page = 1)
        {
            var userId = GetUserId(claims);
            var result = await _repo.GetInternshipsForPersonAsync(
                userId,
                cancellation,
                searchText,
                from,
                to,
                orderBy,
                ascending,
                maxItems,
                page
                );
            return new ResponseItems<PersonInternshipResp>
            {
                Items = result.Items.Select(x =>
                    new PersonInternshipResp(x.Intership, x.Details)).ToList(),
                TotalCount = result.TotalCount,
            };
        }



        public async Task<ResponseItems<CompanyRecruitmentResp>> GetCompanyRecruitmentsAsync(
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
            int page = 1)
        {
            var companyId = GetUserId(claims);
            var result = await _repo.GetCompanyRecruitmentsAsync(
                companyId,
                cancellation,
                searchText,
                from,
                to,
                filterStatus,
                status,
                orderBy,
                ascending,
                maxItems,
                page);

            return new ResponseItems<CompanyRecruitmentResp>
            {
                Items = result.Items.Select(x => new CompanyRecruitmentResp(x)).ToList(),
                TotalCount = result.TotalCount,
            };
        }

        public async Task<ResponseItems<PersonRecruitmentResp>> GetPersonRecruitmentsAsync(
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
            int page = 1)
        {
            var personId = GetUserId(claims);
            var result = await _repo.GetPersonRecruitmentsAsync(
                personId,
                cancellation,
                searchText,
                from,
                to,
                filterStatus,
                status,
                orderBy,
                ascending,
                maxItems,
                page);

            return new ResponseItems<PersonRecruitmentResp>
            {
                Items = result.Items.Select(x => new PersonRecruitmentResp(x)).ToList(),
                TotalCount = result.TotalCount,
            };
        }

        //===============================================================================================
        //===============================================================================================
        //===============================================================================================
        //Private Methods
        private UserId GetUserId(IEnumerable<Claim> claims) => _jwt.GetIdNameFromClaims(claims);

    }
}
