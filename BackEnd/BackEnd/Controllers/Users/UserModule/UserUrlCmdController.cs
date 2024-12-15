using Application.Features.Users.Commands.Urls.DTOs;
using Application.Features.Users.Commands.Urls.DTOs.Update;
using Application.Features.Users.Commands.Urls.Services;
using Application.Shared.DTOs.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers.Users.UserModule
{
    [Route("api/User/urls")]
    [ApiController]
    public class UserUrlCmdController : ControllerBase
    {
        //Values
        private readonly IUrlCmdSvc _urlService;


        public UserUrlCmdController(IUrlCmdSvc urlService)
        {
            _urlService = urlService;
        }


        //=============================================================================================
        //=============================================================================================
        //=============================================================================================
        //Public Methods
        [Authorize]
        [HttpPost("urls")]
        public async Task<IActionResult> CreateAsync
            (
            IEnumerable<CreateUrlReq> dtos,
            CancellationToken cancellation
            )
        {
            var claims = User.Claims.ToList();
            var result = await _urlService.CreateAsync(claims, dtos, cancellation);
            if (result.Status == EnumResponseStatus.UserFault)
            {
                return StatusCode(400, result);
            }
            return StatusCode(201, result);
        }

        [Authorize]
        [HttpPut("urls")]
        public async Task<IActionResult> UpdateAsync
            (
            IEnumerable<UpdateUrlReq> dtos,
            CancellationToken cancellation
            )
        {
            var claims = User.Claims.ToList();
            var result = await _urlService.UpdateAsync(claims, dtos, cancellation);
            if (result.Status == EnumResponseStatus.UserFault)
            {
                return StatusCode(400, result);
            }
            return StatusCode(200, result);
        }

        [Authorize]
        [HttpDelete("urls")]
        public async Task<IActionResult> DeleteAsync
            (
            IEnumerable<DeleteUrlReq> dtos,
            CancellationToken cancellation
            )
        {
            var claims = User.Claims.ToList();
            var result = await _urlService.DeleteAsync(claims, dtos, cancellation);
            if (result.Status == EnumResponseStatus.UserFault)
            {
                return StatusCode(400, result);
            }
            return StatusCode(200, result);
        }
        //=============================================================================================
        //=============================================================================================
        //=============================================================================================
        //Private Methods
    }
}
