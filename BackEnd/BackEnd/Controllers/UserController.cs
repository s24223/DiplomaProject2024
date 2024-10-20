using Application.Features.User.DTOs.CommandsUrl.Create;
using Application.Features.User.DTOs.CommandsUrl.Update;
using Application.Features.User.DTOs.CommandsUser.Create;
using Application.Features.User.DTOs.CommandsUser.LoginIn;
using Application.Features.User.DTOs.CommandsUser.Refresh;
using Application.Features.User.DTOs.CommandsUser.UpdateLogin;
using Application.Features.User.DTOs.CommandsUser.UpdatePassword;
using Application.Features.User.Services.CommandsUrl;
using Application.Features.User.Services.CommandsUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        //Values
        private readonly IUrlCommandService _urlService;
        private readonly IUserCommandService _userService;

        //Cosntructors
        public UserController
            (
            IUrlCommandService urlService,
            IUserCommandService userService

            )
        {
            _urlService = urlService;
            _userService = userService;
        }


        //================================================================================================================
        //================================================================================================================
        //================================================================================================================
        //Public Methods


        //================================================================================================================
        //User Part
        //DML
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



        //================================================================================================================
        //UserProblem Part
        //DML
        /*[AllowAnonymous]
        [HttpPost("problems/unauthorized")]
        public async Task<IActionResult> CreateForUnauthorizedAsync
            (
            CreateUnauthorizedUserProblemRequestDto dto,
            CancellationToken cancellation
            )
        {
            var result = await _userProblemService.CreateForUnauthorizedAsync(dto, cancellation);
            return StatusCode(201, result);
        }


        [Authorize]
        [HttpPost("problems/authorized")]
        public async Task<IActionResult> CreateForAuthorizedAsync
            (
            CreateAuthorizedUserProblemRequestDto dto,
            CancellationToken cancellation
            )
        {
            var claims = User.Claims.ToList();
            var result = await _userProblemService.CreateForAuthorizedAsync(claims, dto, cancellation);
            return StatusCode(201, result);
        }

        [Authorize]
        [HttpPut("problems/authorized/{idUserProblem:guid}/annul")]
        public async Task<IActionResult> AnnulForAuthorizedAsync
           (
           Guid idUserProblem,
           CancellationToken cancellation
           )
        {
            var claims = User.Claims.ToList();
            var result = await _userProblemService.AnnulForAuthorizedAsync(claims, idUserProblem, cancellation);
            return StatusCode(200, result);
        }
*/


        //================================================================================================================
        //Url Part
        //DML
        [Authorize]
        [HttpPost("urls")]
        public async Task<IActionResult> CreateAsync
            (
            CreateUrlRequestDto dto,
            CancellationToken cancellation
            )
        {
            var claims = User.Claims.ToList();
            var result = await _urlService.CreateAsync(claims, dto, cancellation);
            return StatusCode(201, result);
        }

        [Authorize]
        [HttpPut("urls/{urlTypeId:int}&{created:datetime}")]
        public async Task<IActionResult> UpdateAsync
            (
            int urlTypeId,
            DateTime created,
            UpdateUrlRequestDto dto,
            CancellationToken cancellation
            )
        {
            var claims = User.Claims.ToList();
            var result = await _urlService.UpdateAsync(claims, urlTypeId, created, dto, cancellation);
            return StatusCode(200, result);
        }

        [Authorize]
        [HttpDelete("urls/{urlTypeId:int}&{created:datetime}")]
        public async Task<IActionResult> DeleteAsync
            (
            int urlTypeId,
            DateTime created,
            CancellationToken cancellation
            )
        {
            var claims = User.Claims.ToList();
            var result = await _urlService.DeleteAsync(claims, urlTypeId, created, cancellation);
            return StatusCode(200, result);
        }

        //DQL
        [Authorize]
        [HttpGet("urls/types")]
        public IActionResult GetUrlTypes()
        {
            return StatusCode(200, _urlService.GetUrlTypes());
        }


        //================================================================================================================
        //================================================================================================================
        //================================================================================================================
        //Private Methods
    }
}
