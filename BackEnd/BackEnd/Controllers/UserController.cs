using Application.VerticalSlice.UserPart.DTOs.CreateProfile;
using Application.VerticalSlice.UserPart.DTOs.LoginIn;
using Application.VerticalSlice.UserPart.DTOs.Refresh;
using Application.VerticalSlice.UserPart.Services;
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
            if (result.IsSuccess)
            {
                return Ok(result.MessageForUser);
            }
            else if (result.IsUserFault)
            {
                return StatusCode(400, result.MessageForUser);
            }
            else
            {
                return StatusCode(500, result.MessageForUser);
            }
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
            if (result.IsSuccess)
            {
                return Ok(result.Item);
            }
            else if (result.IsUserFault)
            {
                return StatusCode(400, result.MessageForUser);
            }
            else
            {
                return StatusCode(500, result.MessageForUser);
            }
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

                if (result.IsSuccess)
                {
                    return Ok(result.Item);
                }
                else if (result.IsUserFault)
                {
                    return StatusCode(401, result.MessageForUser);
                }
                else
                {
                    return StatusCode(500, result.MessageForUser);
                }
            }
            return StatusCode(401);
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
            if (result.IsSuccess)
            {
                return Ok(result.MessageForUser);
            }
            else if (result.IsUserFault)
            {
                return StatusCode(401, result.MessageForUser);
            }
            else
            {
                return StatusCode(500, result.MessageForUser);
            }
        }
    }
}
