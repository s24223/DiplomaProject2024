using Application.Features.User.DTOs.CommandsNotification.Create.Authorize;
using Application.Features.User.DTOs.CommandsNotification.Create.Unauthorize;
using Application.Features.User.DTOs.CommandsUrl.Create;
using Application.Features.User.DTOs.CommandsUrl.Delete;
using Application.Features.User.DTOs.CommandsUrl.Update;
using Application.Features.User.DTOs.CommandsUser.Create;
using Application.Features.User.DTOs.CommandsUser.LoginIn;
using Application.Features.User.DTOs.CommandsUser.Refresh;
using Application.Features.User.DTOs.CommandsUser.UpdateLogin;
using Application.Features.User.DTOs.CommandsUser.UpdatePassword;
using Application.Features.User.Services.CommandsNotification;
using Application.Features.User.Services.CommandsUrl;
using Application.Features.User.Services.CommandsUser;
using Application.Features.User.Services.QueriesUser;
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
        private readonly IUserQueriesService _queriesService;
        private readonly INotificationCommandService _notificationService;

        //Cosntructors
        public UserController
            (
            IUrlCommandService urlService,
            IUserCommandService userService,
            IUserQueriesService queriesService,
            INotificationCommandService notificationService
            )
        {
            _urlService = urlService;
            _userService = userService;
            _queriesService = queriesService;
            _notificationService = notificationService;
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

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetDataAsync(CancellationToken cancellation)
        {
            var claims = User.Claims.ToList();
            var result = await _queriesService.GetPersonalDataAsync(claims, cancellation);
            return Ok(result);
        }


        //================================================================================================================
        //UserProblem Part
        //DML
        [AllowAnonymous]
        [HttpPost("notifications/unauthorized")]
        public async Task<IActionResult> CreateForUnauthorizedAsync
            (
            CreateUnauthorizeNotificationRequestDto dto,
            CancellationToken cancellation
            )
        {
            var result = await _notificationService.CreateUnauthorizeAsync(dto, cancellation);
            return StatusCode(201, result);
        }


        [Authorize]
        [HttpPost("notifications/authorized")]
        public async Task<IActionResult> CreateForAuthorizedAsync
            (
            CreateAuthorizeNotificationRequestDto dto,
            CancellationToken cancellation
            )
        {
            var claims = User.Claims.ToList();
            var result = await _notificationService.CreateAuthorizeAsync(claims, dto, cancellation);
            return StatusCode(201, result);
        }

        [Authorize]
        [HttpPut("notifications/authorized/{notificationId:guid}/annul")]
        public async Task<IActionResult> AnnulForAuthorizedAsync
           (
            Guid notificationId,
            CancellationToken cancellation
           )
        {
            var claims = User.Claims.ToList();
            var result = await _notificationService.AnnulAsync(claims, notificationId, cancellation);
            return StatusCode(200, result);
        }

        [Authorize]
        [HttpPut("notifications/authorized/{notificationId:guid}/read")]
        public async Task<IActionResult> ReadForAuthorizedAsync
           (
            Guid notificationId,
            CancellationToken cancellation
           )
        {
            var claims = User.Claims.ToList();
            var result = await _notificationService.ReadAsync(claims, notificationId, cancellation);
            return StatusCode(200, result);
        }


        //================================================================================================================
        //Url Part
        //DML
        [Authorize]
        [HttpPost("urls")]
        public async Task<IActionResult> CreateAsync
            (
            IEnumerable<CreateUrlRequestDto> dtos,
            CancellationToken cancellation
            )
        {
            var claims = User.Claims.ToList();
            var result = await _urlService.CreateAsync(claims, dtos, cancellation);
            return StatusCode(201, result);
        }

        [Authorize]
        [HttpPut("urls")]
        public async Task<IActionResult> UpdateAsync
            (
            IEnumerable<UpdateUrlRequestDto> dtos,
            CancellationToken cancellation
            )
        {
            var claims = User.Claims.ToList();
            var result = await _urlService.UpdateAsync(claims, dtos, cancellation);
            return StatusCode(200, result);
        }

        [Authorize]
        [HttpDelete("urls")]
        public async Task<IActionResult> DeleteAsync
            (
            IEnumerable<DeleteUrlRequestDto> dtos,
            CancellationToken cancellation
            )
        {
            var claims = User.Claims.ToList();
            var result = await _urlService.DeleteAsync(claims, dtos, cancellation);
            return StatusCode(200, result);
        }



        //================================================================================================================
        //================================================================================================================
        //================================================================================================================
        //Private Methods
    }
}
