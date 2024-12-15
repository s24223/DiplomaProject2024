using Application.Features.Users.Commands.Notifications.DTOs.Create;
using Application.Features.Users.Commands.Notifications.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers.Users.UserModule
{
    [Route("api/User/notifications")]
    [ApiController]
    public class UserNotificationCmdController : ControllerBase
    {
        //Values
        private readonly INotificationCmdSvc _notificationService;


        //Constructor
        public UserNotificationCmdController(INotificationCmdSvc notificationService)
        {
            _notificationService = notificationService;
        }


        //=============================================================================================
        //=============================================================================================
        //=============================================================================================
        //Public Methods

        [AllowAnonymous]
        [HttpPost("unauthorized")]
        public async Task<IActionResult> CreateForUnauthorizedAsync
            (
            CreateUnAuthNotificationReq dto,
            CancellationToken cancellation
            )
        {
            var result = await _notificationService.CreateUnauthorizeAsync(dto, cancellation);
            return StatusCode(201, result);
        }

        [Authorize]
        [HttpPost("authorized")]
        public async Task<IActionResult> CreateForAuthorizedAsync
            (
            CreateAuthNotificationReq dto,
            CancellationToken cancellation
            )
        {
            var claims = User.Claims.ToList();
            var result = await _notificationService.CreateAuthorizeAsync(claims, dto, cancellation);
            return StatusCode(201, result);
        }


        //Only for Users
        [Authorize]
        [HttpPut("authorized/{notificationId:guid}/annul")]
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
        [HttpPut("authorized/{notificationId:guid}/read")]
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
        //=============================================================================================
        //=============================================================================================
        //=============================================================================================
        //Private Methods
    }
}
