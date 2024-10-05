using Application.VerticalSlice.UserProblemPart.DTOs.Create.Authorized;
using Application.VerticalSlice.UserProblemPart.DTOs.Create.Unauthorized;
using Application.VerticalSlice.UserProblemPart.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProblemsController : ControllerBase
    {
        private readonly IUserProblemService _service;

        public UserProblemsController
            (
            IUserProblemService service
            )
        {
            _service = service;
        }

        [AllowAnonymous]
        [HttpPost("unauthorizedUser/create")]
        public async Task<IActionResult> CreateForUnauthorizedAsync
            (
            CreateUnauthorizedUserProblemRequestDto dto,
            CancellationToken cancellation
            )
        {
            return Ok(await _service.CreateForUnauthorizedAsync(dto, cancellation));
        }


        [Authorize]
        [HttpPost("authorizedUser/create")]
        public async Task<IActionResult> CreateForAuthorizedAsync
            (
            CreateAuthorizedUserProblemRequestDto dto,
            CancellationToken cancellation
            )
        {
            var claims = User.Claims.ToList();
            return Ok(await _service.CreateForAuthorizedAsync(claims, dto, cancellation));
        }

        [Authorize]
        [HttpPut("authorizedUser/{idUserProblem:guid}/annul")]
        public async Task<IActionResult> AnnulForAuthorizedAsync
           (
           Guid idUserProblem,
           CancellationToken cancellation
           )
        {
            var claims = User.Claims.ToList();
            return Ok(await _service.AnnulUserProblemForAuthorizedAsync(claims, idUserProblem, cancellation));
        }
    }
}
