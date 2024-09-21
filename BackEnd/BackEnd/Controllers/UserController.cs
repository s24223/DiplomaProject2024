using Application.VerticalSlice.UserPart.DTOs.CreateProfile;
using Application.VerticalSlice.UserPart.DTOs.LoginIn;
using Application.VerticalSlice.UserPart.DTOs.Refresh;
using Application.VerticalSlice.UserPart.Services;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("create")]
        public async Task<IActionResult> CreateUserProfileAsync
            (
            CreateProfileRequestDto dto,
            CancellationToken cancellation
            )
        {
            var result = await _userService.CreateProfileAsync(dto, cancellation);
            return StatusCode(201, result);
        }

        [AllowAnonymous]
        [HttpPost("loginIn")]
        public async Task<IActionResult> LoginInAsync
            (
            LoginInRequestDto dto,
            CancellationToken cancellation
            )
        {
            var result = await _userService.LoginInAsync(dto, cancellation);
            return Ok(result.Item);
        }

        [AllowAnonymous]
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshAsync
            (
            RefreshRequestDto dto,
            CancellationToken cancellation
            )
        {
            if (Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
            {
                var jwt = authorizationHeader.ToString().Replace("Bearer ", "");
                var result = await _userService.RefreshTokenAsync(jwt, dto, cancellation);
                return Ok(result.Item);
            }
            return StatusCode(401);
        }

        [AllowAnonymous]
        [HttpPost("problemA")]
        public async Task<IActionResult> SubmitProblem(string problemMessage, string email, CancellationToken cancellation)
        {
            var emailObject = new Email(email);
            return Ok(await _userService.CreateUserProblemUnauthorizedAsync(problemMessage, emailObject, cancellation));
        }

        [Authorize]
        [HttpPost("logOut")]
        public async Task<IActionResult> LogOutAsync
            (
            CancellationToken cancellation
            )
        {
            var claims = User.Claims.ToList();
            var result = await _userService.LogOutAsync(claims, cancellation);
            return Ok(result);
        }

        [Authorize]
        [HttpPost("problem")]
        public async Task<IActionResult> SubmitProblem
            (
            string problemMessage,
            CancellationToken cancellation
            )
        {
            var claims = User.Claims.ToList();
            return Ok(await _userService.CreateUserProblemAuthorizedAsync(claims, problemMessage, cancellation));
        }
    }
}
