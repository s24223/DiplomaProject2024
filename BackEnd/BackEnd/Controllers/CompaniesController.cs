using Application.Features.Companies.Queries.PublicCompany.Services;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        //values
        private readonly ICompanyQuerySvc _svc;


        //Constructor
        public CompaniesController(ICompanyQuerySvc svc)
        {
            _svc = svc;
        }


        //================================================================================================
        //================================================================================================
        //================================================================================================
        //Public Methods
        [HttpGet]
        public async Task<IActionResult> GetCompaniesAsync(
           CancellationToken cancellation,
           [FromHeader] IEnumerable<int> characteristics,
           string? companyName = null,
           string? regon = null,
           string? searchText = null,
           string orderBy = "name", // characteristics
           bool ascending = true,
           bool hasActiveOffers = false,
           int maxItems = 100,
           int page = 1)
        {
            var result = await _svc.GetCompaniesAsync(
                cancellation, characteristics, companyName, regon, searchText,
                orderBy, ascending, hasActiveOffers, maxItems, page);
            return Ok(result);
        }

        /*
                [HttpGet("{companyId:guid}/urls")]
                [HttpGet("{companyUrlSegment}/urls")]
                public async Task<IActionResult> GetCompaniesAsync(
                    Guid? companyId,
                    string? companyUrlSegment,
                    CancellationToken cancellation,
                    string? searchText = null,
                    string orderBy = "created", //typeId, name
                    bool ascending = true,
                    int maxItems = 100,
                    int page = 1)
                {
                    var result = await _svc.GetUrlsAsync(companyId,
                        companyUrlSegment, cancellation,
                        searchText, orderBy, ascending, maxItems, page);
                    return StatusCode(200, result);
                }
        */

        [HttpGet("{companyId:guid}/urls")]
        public async Task<IActionResult> GetCompaniesAsync(
            Guid companyId,
            CancellationToken cancellation,
            string? searchText = null,
            string orderBy = "created", //typeId, name
            bool ascending = true,
            int maxItems = 100,
            int page = 1)
        {
            var result = await _svc.GetUrlsAsync(companyId, cancellation,
                searchText, orderBy, ascending, maxItems, page);
            return StatusCode(200, result);
        }

        [HttpGet("{companyId:guid}/offers")]
        public async Task<IActionResult> GetOffersAsync(
           Guid companyId,
           CancellationToken cancellation,
           [FromHeader] IEnumerable<int> characteristics,
           string? searchText = null,
           decimal? minSalary = null,
           decimal? maxSalary = null,
           bool? isForStudents = null,
           bool? isNegotiatedSalary = null,
           bool? hasActive = null,
           string orderBy = "publishStart",
           bool ascending = true,
           int maxItems = 100,
           int page = 1)
        {
            var result = await _svc.GetOffersAsync(companyId, cancellation, characteristics, searchText,
               minSalary, maxSalary, isForStudents, isNegotiatedSalary, hasActive, orderBy, ascending,
               maxItems, page);
            return StatusCode(200, result);
        }

        [HttpGet("{companyId:guid}/branch")]
        public async Task<IActionResult> GGetBranchesAsync(
            Guid companyId,
            CancellationToken cancellation,
            string? wojewodztwo = null,
            string? divisionName = null,
            string? streetName = null,
            string? searchText = null,
            string orderBy = "hierarchy",
            bool ascending = true,
            int maxItems = 100,
            int page = 1)
        {
            var result = await _svc.GetBranchesAsync(companyId, cancellation,
                wojewodztwo, divisionName, streetName, searchText, orderBy, ascending, maxItems, page);

            return StatusCode(200, result);
        }


        //================================================================================================
        //================================================================================================
        //================================================================================================
        //Private Methods
    }
}
