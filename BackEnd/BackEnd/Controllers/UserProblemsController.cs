using Application.VerticalSlice.UserProblemPart.DTOs.CreateUserProblemAuthorized;
using Application.VerticalSlice.UserProblemPart.DTOs.CreateUserProblemUnauthorized;
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
        [HttpPost("createForUnauthorized")]
        public async Task<IActionResult> SubmitProblem
            (
            CreateUnauthorizedUserProblemRequestDto dto,
            CancellationToken cancellation
            )
        {
            return Ok(await _service.CreateForUnauthorizedAsync(dto, cancellation));
        }


        [Authorize]
        [HttpPost("createForAuthorized")]
        public async Task<IActionResult> SubmitProblem
            (
            CreateAuthorizedUserProblemRequestDto dto,
            CancellationToken cancellation
            )
        {
            var claims = User.Claims.ToList();
            return Ok(await _service.CreateForAuthorizedAsync(claims, dto, cancellation));
        }
    }
}
