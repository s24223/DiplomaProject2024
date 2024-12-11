using Application.Features.Internships.Queries.DTOs;
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
        public async Task<ResponseItem<InternshipWithCommentsResp>> CommentsFirstPageAsync(
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

            return new ResponseItem<InternshipWithCommentsResp>
            {
                Item = new InternshipWithCommentsResp(
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
        //===============================================================================================
        //===============================================================================================
        //===============================================================================================
        //Private Methods
        private UserId GetUserId(IEnumerable<Claim> claims) => _jwt.GetIdNameFromClaims(claims);

    }
}
