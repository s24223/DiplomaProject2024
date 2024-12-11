using Application.Features.Internships.Commands.Comments.DTOs;
using Application.Features.Internships.Commands.Comments.Interfaces;
using Application.Shared.DTOs.Features.Internships.Comments;
using Application.Shared.DTOs.Response;
using Application.Shared.Services.Authentication;
using Domain.Features.Comment.ValueObjects.CommentTypePart;
using Domain.Features.User.ValueObjects.Identificators;
using Domain.Shared.Factories;
using System.Security.Claims;

namespace Application.Features.Internships.Commands.Comments.Services
{
    public class CommentSvc : ICommentSvc
    {
        //Values
        private readonly IDomainFactory _factory;
        private readonly ICommentRepo _repo;
        private readonly IAuthJwtSvc _jwtSvc;


        //Constructor
        public CommentSvc
            (
            IDomainFactory factory,
            ICommentRepo repo,
            IAuthJwtSvc jwtSvc
            )
        {
            _factory = factory;
            _jwtSvc = jwtSvc;
            _repo = repo;
        }


        //===============================================================================================
        //===============================================================================================
        //===============================================================================================
        //Public Methods
        public async Task<ResponseItem<CommentResp>> CreateWithEvaluationAsync
            (
            IEnumerable<Claim> claims,
            Guid intershipId,
            CreateCommentEvaluationReq dto,
            CancellationToken cancellation
            )
        {
            return await CreateAsync(claims, intershipId, dto, cancellation);
        }

        public async Task<ResponseItem<CommentResp>> CreateWithOutEvaluationAsync
            (
            IEnumerable<Claim> claims,
            Guid intershipId,
            CreateCommentReq dto,
            CancellationToken cancellation
            )
        {
            return await CreateAsync(claims, intershipId, dto, cancellation);
        }

        public Dictionary<int, string> GetCommentTypesWithEvaluation()
        {
            return GetCommentType(val => val < 1002);
        }

        public Dictionary<int, string> GetCommentTypesWithOutEvaluation()
        {
            return GetCommentType(val => val >= 1002);
        }

        //===============================================================================================
        //===============================================================================================
        //===============================================================================================
        //Public Methods
        private UserId GetId(IEnumerable<Claim> claims)
        {
            return _jwtSvc.GetIdNameFromClaims(claims);
        }


        private async Task<ResponseItem<CommentResp>> CreateAsync(
            IEnumerable<Claim> claims,
            Guid intershipId,
            CreateCommentReq dto,
            CancellationToken cancellation)
        {
            var id = GetId(claims);
            var sender = await _repo.GetSenderRoleAsync(intershipId, id, cancellation);
            int? evaluation = (dto is CreateCommentEvaluationReq evReq) ?
                evReq.Evaluation : null;

            var comment = _factory.CreateDomainComment
                (
                intershipId,
                sender,
                (CommentTypeEnum)dto.CommentTypeId,
                dto.Description,
                evaluation
                );
            comment = await _repo.CreateCommentAsync(comment, cancellation);

            return new ResponseItem<CommentResp>
            {
                Item = new CommentResp(comment)
            };
        }

        private Dictionary<int, string> GetCommentType(Func<int, bool> filter)
        {

            var values = Enum.GetValues(typeof(CommentTypeEnum));
            var dictionary = new Dictionary<int, string>();
            foreach (var value in values)
            {
                var id = (int)value;
                if (filter(id))
                {
                    if ((int)value == 1004)
                    {
                        dictionary.Add((int)value, "Pozwolenie na publikację");
                    }
                    else
                    {
                        dictionary.Add((int)value, value.ToString() ?? "");
                    }
                }
            }
            return dictionary;
        }
    }
}
