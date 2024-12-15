using Application.Features.Users.Commands.Users.DTOs.Create;
using Application.Features.Users.Commands.Users.DTOs.LoginIn;
using Application.Features.Users.Commands.Users.DTOs.Refresh;
using Application.Features.Users.Commands.Users.DTOs.UpdateLogin;
using Application.Features.Users.Commands.Users.DTOs.UpdatePassword;
using Application.Features.Users.Commands.Users.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers.Users.UserModule
{
    [Route("api/User")]
    [ApiController]
    public class UserCmdController : ControllerBase
    {
        //Values
        private readonly IUserCommandService _userService;


        //Constructor
        public UserCmdController(IUserCommandService userService)
        {
            _userService = userService;
        }


        //================================================================================================================
        //================================================================================================================
        //================================================================================================================
        //Public Methods
        [AllowAnonymous]
        [HttpPost()]
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
        [HttpPost("login")]
        public async Task<IActionResult> LoginInAsync
            (
            LoginInRequestDto dto,
            CancellationToken cancellation
            )
        {
            var result = await _userService.LoginInAsync(dto, cancellation);
            return Ok(result.Item);
        }

        [Authorize]
        [HttpPut("login")]
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

        //================================================================================================================
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

        //================================================================================================================
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

        //================================================================================================================
        [Authorize]
        [HttpPut("password")]
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
