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
        private readonly IUserCompanyQuerySvc _userCompanySvc;


        //Constructor
        public UserCompanyQueryController(IUserCompanyQuerySvc userCompanySvc)
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

        [Authorize]
        [HttpGet()]
        public async Task<IActionResult> GetCompanyAsync(
            [FromHeader] IEnumerable<int> characteristics,
            CancellationToken cancellation,
            int? divisionId = null,
            int? streetId = null,
            bool ascendingBranch = true,
            int itemsCountBranch = 100,
            int pageBranch = 1,
            string? searchText = null,
            bool? isNegotiatedSalary = null,
            bool? isForStudents = null,
            decimal? minSalary = null,
            decimal? maxSalary = null,
            string orderByOffer = "created",
            bool ascendingOffer = true,
            int itemsCountOffer = 100,
            int pageOffer = 1)
        {
            var claims = User.Claims.ToList();
            var data = await _userCompanySvc.GetCompanyAsync
                (
                claims,
                characteristics,
                cancellation,
                divisionId,
                streetId,
                ascendingBranch,
                itemsCountBranch,
                pageBranch,
                searchText,
                isNegotiatedSalary,
                isForStudents,
                minSalary,
                maxSalary,
                orderByOffer,
                ascendingOffer,
                itemsCountOffer,
                pageOffer);
            return StatusCode(200, data);
        }

        [Authorize]
        [HttpGet("branches/details")]
        public async Task<IActionResult> GetBranchesWithDetailsAsync(
            CancellationToken cancellation,
            int? divisionId = null,
            int? streetId = null,
            bool ascending = true,
            int itemsCount = 100,
            int page = 1)
        {
            var claims = User.Claims.ToList();
            var data = await _userCompanySvc.GetBranchesWithDetailsAsync
                (
                claims,
                cancellation,
                divisionId,
                streetId,
                ascending,
                itemsCount,
                page
                );
            return StatusCode(200, data);
        }

        [Authorize]
        [HttpGet("offers/details")]
        public async Task<IActionResult> GetOfferWithDetailsAsync(
            [FromHeader] IEnumerable<int> characteristics,
            CancellationToken cancellation,
            string? searchText = null,
            bool? isNegotiatedSalary = null,
            bool? isForStudents = null,
            decimal? minSalary = null,
            decimal? maxSalary = null,
            string orderBy = "created",
            bool ascending = true,
            int itemsCount = 100,
            int page = 1)
        {
            var claims = User.Claims.ToList();
            var data = await _userCompanySvc.GetOfferWithDetailsAsync(
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
                page);
            return StatusCode(200, data);
        }
        //================================================================================================================
        //branches&offers

        //================================================================================================================
        //================================================================================================================
        //Private Methods


    }
}
