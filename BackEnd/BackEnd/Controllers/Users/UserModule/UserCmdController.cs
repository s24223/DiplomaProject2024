using Application.Features.Users.Commands.Users.DTOs.Create;
using Application.Features.Users.Commands.Users.DTOs.LoginIn;
using Application.Features.Users.Commands.Users.DTOs.Refresh;
using Application.Features.Users.Commands.Users.DTOs.ResetPassword;
using Application.Features.Users.Commands.Users.DTOs.ResetPasswordLink;
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
            CreateUserReq dto,
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
            LoginInReq dto,
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
            UpdateLoginReq dto,
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
            RefreshReq dto,
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
            UpdatePasswordReq dto,
            CancellationToken cancellation
            )
        {
            var claims = User.Claims.ToList();
            var result = await _userService.UpdatePasswordAsync(claims, dto, cancellation);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("activate/{id:guid}/{activationUrlSegment}")]
        public async Task<IActionResult> ActivateAsync
            (
            Guid id, string activationUrlSegment, CancellationToken cancellation
            )
        {
            var result = await _userService.ActivateAsync(id, activationUrlSegment, cancellation);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("reset")]
        public async Task<IActionResult> ResetPasswordInitiateAsync(
            ResetPasswordLinkReq req,
            CancellationToken cancellation
            )
        {
            var result = await _userService.ResetPasswordInitiateAsync(req, cancellation);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("reset/{id:guid}/{resetPasswordUrlSegment}")]
        public async Task<IActionResult> ResetPasswordAsync(
            Guid id,
            string resetPasswordUrlSegment,
            ResetPasswordReq req,
            CancellationToken cancellation)
        {
            var result = await _userService.ResetPasswordAsync(
                id,
                resetPasswordUrlSegment,
                req,
                cancellation);
            return Ok(result);
        }
    }
}
