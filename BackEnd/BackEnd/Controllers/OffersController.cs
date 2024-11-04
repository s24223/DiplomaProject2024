using Application.Features.Companies.Services.QueriesCompany;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OffersController : ControllerBase
    {
        //Values
        private readonly ICompanyQueryService _companyQuery;


        //Cnstructor
        public OffersController
            (
            ICompanyQueryService companyQuery
            )
        {
            _companyQuery = companyQuery;
        }


        //=====================================================================================================
        //=====================================================================================================
        //=====================================================================================================
        //Public Methods
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetOfferAsync
            (
            Guid id,
            CancellationToken cancellation
            )
        {
            var result = await _companyQuery.GetOfferAsync
                (
                id,
                cancellation
                );
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync
            (
            Guid? companyId,
            int? divisionId,
            [FromHeader] IEnumerable<int> characteristics,
            CancellationToken cancellation,
            bool? isPayed = null,
            DateTime? publishStart = null,
            DateTime? publishEnd = null,
            DateTime? workStart = null,
            DateTime? workEnd = null,
            decimal? salaryMin = null,
            decimal? salaryMax = null,
            bool? isNegotietedSalary = null,
            string orderBy = "publishStart",
            bool ascending = true,
            int maxItems = 100,
            int page = 1
            )
        {
            var result = await _companyQuery.GetOffersAsync
                (
                companyId,
                divisionId,
                characteristics,
                cancellation,
                isPayed,
                publishStart,
                publishEnd,
                workStart,
                workEnd,
                salaryMin,
                salaryMax,
                isNegotietedSalary,
                orderBy,
                ascending,
                maxItems,
                page
                );
            return Ok(result);
        }

        [HttpGet("companies/{companyId:guid}/branches/{branchId:guid}")]
        [HttpGet("companies/{companyUrlsegment}/branches/{branchId:guid}")]
        [HttpGet("companies/{companyUrlsegment}/branches/{branchUrlSegment}")]
        [HttpGet("companies/{companyId:guid}/branches/{branchUrlSegment}")]
        public async Task<IActionResult> GetOffersByBranchAsync
            (
            Guid? companyId,
            string? companyUrlsegment,
            Guid? branchId,
            string? branchUrlSegment,
            [FromHeader] IEnumerable<int> characteristics,
            CancellationToken cancellation,
            bool? isPayed = null,
            DateTime? publishStart = null,
            DateTime? publishEnd = null,
            DateTime? workStart = null,
            DateTime? workEnd = null,
            decimal? salaryMin = null,
            decimal? salaryMax = null,
            bool? isNegotietedSalary = null,
            string orderBy = "publishStart",
            bool ascending = true,
            int maxItems = 100,
            int page = 1
            )
        {
            var result = await _companyQuery.GetOffersByBranchAsync
                (
                companyId,
                companyUrlsegment,
                branchId,
                branchUrlSegment,
                characteristics,
                cancellation,
                isPayed,
                publishStart,
                publishEnd,
                workStart,
                workEnd,
                salaryMin,
                salaryMax,
                isNegotietedSalary,
                orderBy,
                ascending,
                maxItems,
                page
                );
            return Ok(result);
        }

        //=====================================================================================================
        //=====================================================================================================
        //=====================================================================================================
        //Private Methods
    }
}
