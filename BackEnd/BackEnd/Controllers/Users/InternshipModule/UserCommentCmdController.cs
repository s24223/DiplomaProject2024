using Application.Features.Internships.Commands.Comments.DTOs;
using Application.Features.Internships.Commands.Comments.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers.Users.InternshipModule
{
    [Route("api/User/internship")]
    [ApiController]
    public class UserCommentCmdController : ControllerBase
    {
        //Values
        private readonly ICommentCmdSvc _commentSvc;


        //Constructor
        public UserCommentCmdController(ICommentCmdSvc commentSvc)
        {
            _commentSvc = commentSvc;
        }


        //=============================================================================================
        //=============================================================================================
        //=============================================================================================
        //Public Methods

        [Authorize]
        [HttpPost("{intershipId:guid}/commentWithEvalaution")]
        public async Task<IActionResult> CreateCommentWitEvaluationAsync
            (
            Guid intershipId,
            CreateCommentEvaluationReq dto,
            CancellationToken cancellation
            )
        {
            var claims = User.Claims.ToList();
            var result = await _commentSvc.CreateWithEvaluationAsync
                (
                claims,
                intershipId,
                dto,
                cancellation
                );
            return StatusCode(200, result);
        }

        [Authorize]
        [HttpPost("{internshipId:guid}/commentWithOutEvalaution")]
        public async Task<IActionResult> CreateCommentWitOutEvaluationAsync
            (
            Guid internshipId,
            CreateCommentReq dto,
            CancellationToken cancellation
            )
        {
            var claims = User.Claims.ToList();
            var result = await _commentSvc.CreateWithOutEvaluationAsync
                (
                claims,
                internshipId,
                dto,
                cancellation
                );
            return StatusCode(200, result);
        }
        //=============================================================================================
        //=============================================================================================
        //=============================================================================================
        //Private Methods
    }
}
