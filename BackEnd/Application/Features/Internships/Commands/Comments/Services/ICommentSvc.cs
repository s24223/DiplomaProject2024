using Application.Features.Internships.Commands.Comments.DTOs;
using Application.Shared.DTOs.Features.Internships.Comments;
using Application.Shared.DTOs.Response;
using System.Security.Claims;

namespace Application.Features.Internships.Commands.Comments.Services
{
    public interface ICommentSvc
    {
        Task<ResponseItem<CommentResp>> CreateWithEvaluationAsync
            (
            IEnumerable<Claim> claims,
            Guid intershipId,
            CreateCommentEvaluationReq dto,
            CancellationToken cancellation
            );

        Task<ResponseItem<CommentResp>> CreateWithOutEvaluationAsync
            (
            IEnumerable<Claim> claims,
            Guid intershipId,
            CreateCommentReq dto,
            CancellationToken cancellation
            );

        //Dictionaries
        Dictionary<int, string> GetCommentTypesWithEvaluation();
        Dictionary<int, string> GetCommentTypesWithOutEvaluation();
    }
}
