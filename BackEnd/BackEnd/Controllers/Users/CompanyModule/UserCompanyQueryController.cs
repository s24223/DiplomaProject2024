using Application.Features.Companies.Queries.QueriesUser.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers.Users.CompanyModule
{
    [Route("api/User/company")]
    [ApiController]
    public class UserCompanyQueryController : ControllerBase
    {
        //Values
        private readonly IUserCompanySvc _userCompanySvc;


        //Constructor
        public UserCompanyQueryController(IUserCompanySvc userCompanySvc)
        {
            _userCompanySvc = userCompanySvc;
        }


        //================================================================================================================
        //================================================================================================================
        //================================================================================================================
        //Public Methods
        //================================================================================================================
        //================================================================================================================
        //Company Part

        [Authorize]
        [HttpGet("branches/core")]
        public async Task<IActionResult> UpdateBranchAsync
            (
            CancellationToken cancellation,
            int? divisionId = null,
            int? streetId = null,
            bool ascending = true,
            int itemsCount = 100,
            int page = 1
            )
        {
            var claims = User.Claims.ToList();
            var result = await _userCompanySvc.GetCoreBranchesAsync
                (
                claims,
                cancellation,
                divisionId,
                streetId,
                ascending,
                itemsCount,
                page
                );
            return StatusCode(200, result);
        }


        [Authorize]
        [HttpGet("offers/core")]
        public async Task<IActionResult> UpdateOffersAsync
            (
            [FromHeader]
            IEnumerable<int> characteristics,
            CancellationToken cancellation,
            string? searchText = null,
            bool? isNegotiatedSalary = null,
            bool? isForStudents = null,
            decimal? minSalary = null,
            decimal? maxSalary = null,
            string orderBy = "created",
            bool ascending = true,
            int itemsCount = 100,
            int page = 1
            )
        {
            var claims = User.Claims.ToList();
            var result = await _userCompanySvc.GetCoreOffersAsync
                (
                claims,
                characteristics,
                cancellation,
                searchText,
                isNegotiatedSalary,
                isForStudents,
                minSalary,
                maxSalary,
                orderBy,
                ascending,
                itemsCount,
                page
                );
            return StatusCode(200, result);
        }
        //================================================================================================================
        //branches&offers

        //================================================================================================================
        //================================================================================================================
        //Private Methods


    }
}
