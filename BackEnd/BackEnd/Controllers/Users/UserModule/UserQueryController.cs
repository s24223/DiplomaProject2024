using Application.Features.Users.Queries.QueriesUser.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BackEnd.Controllers.Users.UserModule
{
    [Route("api/User")]
    [ApiController]
    public class UserQueryController : ControllerBase
    {
        //values
        private readonly IUserQuerySvc _userQueryService;


        //Controller
        public UserQueryController(IUserQuerySvc userQueryService)
        {
            _userQueryService = userQueryService;
        }


        //============================================================================================
        //============================================================================================
        //============================================================================================
        //Public Methods
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetDataAsync(CancellationToken cancellation)
        {
            var claims = User.Claims.ToList();
            var result = await _userQueryService.GetUserDataAsync(claims, cancellation);
            return Ok(result);
        }


        [Authorize]
        [HttpGet("notifications/authorized")]
        public async Task<IActionResult> GetNotificationsAsync
           (
            CancellationToken cancellation,
            string? searchText = null,
            bool? hasReaded = null,
            int? senderId = null,
            int? statusId = null,
            DateTime? createdStart = null,
            DateTime? createdEnd = null,
            DateTime? completedStart = null,
            DateTime? completedEnd = null,
            string orderBy = "created", //completed
            bool ascending = true,
            int itemsCount = 100,
            int page = 1
           )
        {
            var claims = User.Claims.ToList();
            var result = await _userQueryService.GetNotificationsAsync
                (
                claims,
                cancellation,
                searchText,
                hasReaded,
                senderId,
                statusId,
                createdStart,
                createdEnd,
                completedStart,
                completedEnd,
                orderBy,
                ascending,
                itemsCount,
                page
                );
            return StatusCode(200, result);
        }

        [Authorize]
        [HttpGet("urls")]
        public async Task<IActionResult> GetUrlsAsync
            (
            CancellationToken cancellation,
            string? searchText = null,
            string orderBy = "created", //typeId, name
            [Required] bool ascending = true,
            int itemsCount = 100,
            int page = 1
            )
        {
            var claims = User.Claims.ToList();
            var result = await _userQueryService
                .GetUrlsAsync(claims, cancellation, searchText, orderBy, ascending, itemsCount, page);
            return StatusCode(200, result);
        }
        //============================================================================================
        //============================================================================================
        //============================================================================================
        //Private Methods
    }
}
