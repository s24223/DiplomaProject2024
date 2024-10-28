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
        [HttpGet]
        public async Task<IActionResult> GetAsync
            (
            CancellationToken cancellation,
            string orderBy = "publishStart",
            bool ascending = true,
            bool? isPayed = null,
            DateTime? publishStart = null,
            DateTime? publishEnd = null,
            DateTime? workStart = null,
            DateTime? workEnd = null,
            Guid? companyId = null,
            Guid? branchId = null,
            Guid? offerId = null,
            decimal? salaryMin = null,
            decimal? salaryMax = null,
            bool? isNegotietedSalary = null,
            int? divisionId = null,
            int maxItems = 100,
            int page = 1
            )
        {
            var result = await _companyQuery.GetOffersAsync
                (
                orderBy,
                ascending,
                isPayed,
                publishStart,
                publishEnd,
                workStart,
                workEnd,
                companyId,
                branchId,
                offerId,
                salaryMin,
                salaryMax,
                isNegotietedSalary,
                divisionId,
                maxItems,
                page,
                cancellation
                );
            return Ok(result);
        }
        //=====================================================================================================
        //=====================================================================================================
        //=====================================================================================================
        //Private Methods
    }
}
