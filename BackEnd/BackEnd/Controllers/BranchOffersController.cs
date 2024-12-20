using Application.Features.Companies.Queries.PublicBranchOffer.Services;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/[controller]")]
    [ApiController]
    public class BranchOffersController : ControllerBase
    {
        //values
        private readonly IBranchOfferQuerySvc _offerQuerySvc;

        //Controller
        public BranchOffersController(IBranchOfferQuerySvc offerQuerySvc)
        {
            _offerQuerySvc = offerQuerySvc;
        }

        //=====================================================================================================
        //=====================================================================================================
        //=====================================================================================================
        //Public Methods

        [HttpGet]
        public async Task<IActionResult> GetBranchOffersAsync(
            CancellationToken cancellation,
            [FromHeader] IEnumerable<int> characteristics,
            string? companyName = null,
            string? regon = null,
            string? wojewodstwo = null,
            string? divisionName = null,
            string? streetName = null,
            string? searchText = null,
            DateTime? publishFrom = null,
            DateTime? publishTo = null,
            DateTime? workFrom = null,
            DateTime? workTo = null,
            decimal? minSalary = null,
            decimal? maxSalary = null,
            bool? isForStudents = null,
            bool? isNegotiatedSalary = null,
            string orderBy = "publishStart",
            bool ascending = true,
            int maxItems = 100,
            int page = 1)
        {
            string? jwt = null;
            if (Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
            {
                jwt = authorizationHeader.ToString().Replace("Bearer ", "");
            }
            var result = await _offerQuerySvc.GetBranchOffersAsync(cancellation, characteristics,
             jwt, companyName, regon, wojewodstwo, divisionName, streetName,
             searchText, publishFrom, publishTo, workFrom, workTo, minSalary, maxSalary, isForStudents,
             isNegotiatedSalary, orderBy, ascending, maxItems, page);

            return StatusCode(200, result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetBranchOfferAsync(
            Guid id,
            CancellationToken cancellation)
        {
            string? jwt = null;
            if (Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
            {
                jwt = authorizationHeader.ToString().Replace("Bearer ", "");
            }
            var result = await _offerQuerySvc.GetBranchOfferAsync(id, cancellation, jwt);
            return StatusCode(200, result);
        }

        [HttpGet("offers/{offerId:guid}")]
        public async Task<IActionResult> GetOfferAsync(
            Guid offerId,
            CancellationToken cancellation,
            string? wojewodstwo = null,
            string? divisionName = null,
            string? streetName = null,
            string? searchText = null,
            DateTime? publishFrom = null,
            DateTime? publishTo = null,
            DateTime? workFrom = null,
            DateTime? workTo = null,
            string orderBy = "publishStart",
            bool ascending = true,
            int maxItems = 100,
            int page = 1)
        {
            string? jwt = null;
            if (Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
            {
                jwt = authorizationHeader.ToString().Replace("Bearer ", "");
            }
            var result = await _offerQuerySvc.GetOfferAsync(offerId, cancellation, jwt, wojewodstwo,
                divisionName, streetName,
                searchText, publishFrom, publishTo, workFrom, workTo, orderBy, ascending, maxItems, page);
            return StatusCode(200, result);
        }

        [HttpGet("offers/{offerId:guid}/branchOffers")]
        public async Task<IActionResult> GetOffersBranchOffersAsync(
            Guid offerId,
            CancellationToken cancellation,
            string? wojewodstwo = null,
            string? divisionName = null,
            string? streetName = null,
            string? searchText = null,
            DateTime? publishFrom = null,
            DateTime? publishTo = null,
            DateTime? workFrom = null,
            DateTime? workTo = null,
            string orderBy = "publishStart",
            bool ascending = true,
            int maxItems = 100,
            int page = 1)
        {
            string? jwt = null;
            if (Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
            {
                jwt = authorizationHeader.ToString().Replace("Bearer ", "");
            }
            var result = await _offerQuerySvc.GetOfferBranchOffersAsync(offerId, cancellation, jwt,
                wojewodstwo, divisionName, streetName,
                searchText, publishFrom, publishTo, workFrom, workTo, orderBy, ascending, maxItems, page);
            return StatusCode(200, result);
        }


        [HttpGet("branches/{branchId:guid}")]
        [HttpGet("companies/{companyId:guid}/branches/{branchUrlSegment}")]
        [HttpGet("companies/{companyUrlSegment}/branches/{branchUrlSegment}")]
        public async Task<IActionResult> GetBranchAsync(
               Guid? companyId,
               Guid? branchId,
               string? companyUrlSegment,
               string? branchUrlSegment,
               CancellationToken cancellation,
               [FromHeader] IEnumerable<int> characteristics,
               string? searchText = null,
               DateTime? publishFrom = null,
               DateTime? publishTo = null,
               DateTime? workFrom = null,
               DateTime? workTo = null,
               decimal? minSalary = null,
               decimal? maxSalary = null,
               bool? isForStudents = null,
               bool? isNegotiatedSalary = null,
               string orderBy = "publishStart",
               bool ascending = true,
               int maxItems = 100,
               int page = 1)
        {
            string? jwt = null;
            if (Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
            {
                jwt = authorizationHeader.ToString().Replace("Bearer ", "");
            }
            var result = await _offerQuerySvc.GetBranchAsync(companyId, branchId,
                companyUrlSegment, branchUrlSegment, cancellation, characteristics,
                jwt, searchText, publishFrom, publishTo, workFrom, workTo, minSalary,
                maxSalary, isForStudents, isNegotiatedSalary, orderBy, ascending,
                maxItems, page);
            return StatusCode(200, result);
        }


        [HttpGet("branches/{branchId:guid}/branchOffers")]
        [HttpGet("companies/{companyId:guid}/branches/{branchUrlSegment}/branchOffers")]
        [HttpGet("companies/{companyUrlSegment}/branches/{branchUrlSegment}/branchOffers")]
        public async Task<IActionResult> GetBranchBranchOffersAsync(
               Guid? companyId,
               Guid? branchId,
               string? companyUrlSegment,
               string? branchUrlSegment,
               CancellationToken cancellation,
               [FromHeader] IEnumerable<int> characteristics,
               string? searchText = null,
               DateTime? publishFrom = null,
               DateTime? publishTo = null,
               DateTime? workFrom = null,
               DateTime? workTo = null,
               decimal? minSalary = null,
               decimal? maxSalary = null,
               bool? isForStudents = null,
               bool? isNegotiatedSalary = null,
               string orderBy = "publishStart",
               bool ascending = true,
               int maxItems = 100,
               int page = 1)
        {
            string? jwt = null;
            if (Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
            {
                jwt = authorizationHeader.ToString().Replace("Bearer ", "");
            }
            var result = await _offerQuerySvc.GetBranchBranchOffersAsync(companyId, branchId,
                companyUrlSegment, branchUrlSegment, cancellation, characteristics,
                jwt, searchText, publishFrom, publishTo, workFrom, workTo, minSalary,
                maxSalary, isForStudents, isNegotiatedSalary, orderBy, ascending,
                maxItems, page);
            return StatusCode(200, result);
        }

        //=====================================================================================================
        //=====================================================================================================
        //=====================================================================================================
        //Private Methods
    }
}
