using Application.Features.Internships.Queries.Users.Servises;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers.Users.InternshipModule
{
    [Route("api/User")]
    [ApiController]
    public class UserInternshipQueryController : ControllerBase
    {
        //Values
        private readonly IUsersInternshipsQuerySvc _userIntershipQuery;


        //Constructor
        public UserInternshipQueryController(IUsersInternshipsQuerySvc svc)
        {
            _userIntershipQuery = svc;
        }


        //=============================================================================================
        //=============================================================================================
        //=============================================================================================
        //Public Methods

        [Authorize]
        [HttpGet("company/recruitment")]
        public async Task<IActionResult> GetCompanyRecruitmentsAsync(
            CancellationToken cancellation,
            string? searchText = null,
            DateTime? from = null,
            DateTime? to = null,
            bool filterStatus = false,
            bool? status = null, // true accepted, false denied
            string orderBy = "created", // ContractStartDate
            bool ascending = true,
            int maxItems = 100,
            int page = 1)
        {
            var claims = User.Claims.ToList();
            var result = await _userIntershipQuery.GetCompanyRecruitmentsAsync(
                claims,
                cancellation,
                searchText,
                from,
                to,
                filterStatus,
                status,
                orderBy,
                ascending,
                maxItems);
            return StatusCode(201, result);
        }

        [Authorize]
        [HttpGet("person/recruitment")]
        public async Task<IActionResult> GetPersonRecruitmentsAsync(
            CancellationToken cancellation,
            string? searchText = null,
            DateTime? from = null,
            DateTime? to = null,
            bool filterStatus = false,
            bool? status = null, // true accepted, false denied
            string orderBy = "created", // ContractStartDate
            bool ascending = true,
            int maxItems = 100,
            int page = 1)
        {
            var claims = User.Claims.ToList();
            var result = await _userIntershipQuery.GetPersonRecruitmentsAsync(
                claims,
                cancellation,
                searchText,
                from,
                to,
                filterStatus,
                status,
                orderBy,
                ascending,
                maxItems);
            return StatusCode(200, result);
        }

        [Authorize]
        [HttpGet("company/branches&offers/{branchOfferId:guid}/recruitments")]
        public async Task<IActionResult> GetPersonRecruitmentsAsync(
            Guid branchOfferId,
            CancellationToken cancellation,
            string? searchText = null,
            DateTime? from = null,
            DateTime? to = null,
            bool filterStatus = false,
            bool? status = null, // true accepted, false denied
            string orderBy = "created", // ContractStartDate
            bool ascending = true,
            int maxItems = 100,
            int page = 1)
        {
            var claims = User.Claims.ToList();
            if (page < 2)
            {
                var result = await _userIntershipQuery.GetBranchOfferRecruitmentsFirstPageAsync(
                        claims,
                        branchOfferId,
                        cancellation,
                        searchText,
                        from,
                        to,
                        filterStatus,
                        status,
                        orderBy,
                        ascending,
                        maxItems);
                return StatusCode(200, result);
            }
            else
            {
                var result = await _userIntershipQuery.GetBranchOfferRecruitmentsAsync(
                        claims,
                        branchOfferId,
                        cancellation,
                        searchText,
                        from,
                        to,
                        filterStatus,
                        status,
                        orderBy,
                        ascending,
                        maxItems,
                        page);
                return StatusCode(200, result);
            }
        }

        [Authorize]
        [HttpGet("person/internships")]
        public async Task<IActionResult> GetInternshipsForPersonAsync(
           CancellationToken cancellation,
           string? searchText = null,
           DateTime? from = null,
           DateTime? to = null,
           string orderBy = "created", // ContractStartDate
           bool ascending = true,
           int maxItems = 100,
           int page = 1)
        {
            var claims = User.Claims.ToList();
            var result = await _userIntershipQuery
                .GetInternshipsForPersonAsync(
                    claims,
                    cancellation,
                    searchText,
                    from,
                    to,
                    orderBy,
                    ascending,
                    maxItems,
                    page);
            return StatusCode(200, result);
        }

        [Authorize]
        [HttpGet("company/internships")]
        public async Task<IActionResult> GetInternshipsForCompanyAsync(
           CancellationToken cancellation,
           string? searchText = null,
           DateTime? from = null,
           DateTime? to = null,
           string orderBy = "created", // ContractStartDate
           bool ascending = true,
           int maxItems = 100,
           int page = 1)
        {
            var claims = User.Claims.ToList();
            var result = await _userIntershipQuery
                .GetInternshipsForCompanyAsync(
                    claims,
                    cancellation,
                    searchText,
                    from,
                    to,
                    orderBy,
                    ascending,
                    maxItems,
                    page);
            return StatusCode(200, result);
        }


        [Authorize]
        [HttpGet("internship/{internshipId:guid}/comments")]
        public async Task<IActionResult> GetCommentsAsync
            (
            Guid internshipId,
            CancellationToken cancellation,
            string? searchText = null,
            int? commentType = null,
            DateTime? from = null,
            DateTime? to = null,
            string orderBy = "created", // CommentTypeId
            bool ascending = false,
            int maxItems = 100,
            int page = 1)
        {
            var claims = User.Claims.ToList();
            if (page < 2)
            {
                var result = await _userIntershipQuery.CommentsFirstPageAsync(
                    claims,
                    internshipId,
                    cancellation,
                    searchText,
                    commentType,
                    from,
                    to,
                    orderBy,
                    ascending,
                    maxItems
                    );
                return StatusCode(200, result);
            }
            else
            {
                var result = await _userIntershipQuery.CommentsAsync(
                    claims,
                    internshipId,
                    cancellation,
                    searchText,
                    commentType,
                    from,
                    to,
                    orderBy,
                    ascending,
                    maxItems,
                    page
                    );
                return StatusCode(200, result);
            }
        }


        [Authorize]
        [HttpGet("cv/{fileId}")]
        public async Task<IActionResult> GetCvAsync(
            string fileId,
            CancellationToken cancellation)
        {
            var claims = User.Claims.ToList();
            var result = await _userIntershipQuery.GetCvAsync(claims, fileId, cancellation);
            if (result == null)
            {
                return NotFound();
            }
            Response.Headers.Add("Content-Disposition", $"attachment; filename*=UTF-8''{result.Value.Name}.pdf");
            Response.ContentType = "application/pdf"; // Możesz dynamicznie ustawić Content-Type na podstawie pliku

            return File(result.Value.Stream, "application/pdf");
        }
        //=============================================================================================
        //=============================================================================================
        //=============================================================================================
        //Private Methods
    }
}
