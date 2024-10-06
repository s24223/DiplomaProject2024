using Application.Features.User.UserPart.DTOs.Create;
using Application.Features.User.UserPart.DTOs.LoginIn;
using Application.Features.User.UserPart.DTOs.Refresh;
using Application.Features.User.UserPart.DTOs.UpdateLogin;
using Application.Features.User.UserPart.DTOs.UpdatePassword;
using Application.Features.User.UserPart.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("create")]
        public async Task<IActionResult> CreateUserProfileAsync
            (
            CreateUserRequestDto dto,
            CancellationToken cancellation
            )
        {
            var result = await _userService.CreateAsync(dto, cancellation);
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
        [HttpPut("updateLogin")]
        public async Task<IActionResult> UpdateLoginAsync
            (
            UpdateLoginRequestDto dto,
            CancellationToken cancellation
            )
        {
            var claims = User.Claims.ToList();
            var result = await _userService.UpdateLoginAsync(claims, dto, cancellation);
            return Ok(result);
        }

        [Authorize]
        [HttpPut("updatePassword")]
        public async Task<IActionResult> UpdatePasswordAsync
            (
            UpdatePasswordRequestDto dto,
            CancellationToken cancellation
            )
        {
            var claims = User.Claims.ToList();
            var result = await _userService.UpdatePasswordAsync(claims, dto, cancellation);
            return Ok(result);
        }
    }
}
