using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Application.Features.Company.OfferBranchPart.Services;
using Application.Features.Company.BranchOfferPart.DTOs.CreateProfile;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BranchOfferController : ControllerBase
    {
        private readonly IBranchOfferService _service;

        public BranchOfferController(IBranchOfferService service)
        {
            _service = service;
        }

        [HttpPost]
        [HttpPost("create")]
        public async Task<IActionResult> CreateBranchOfferAsync(CreateBranchOfferDto dto, CancellationToken cancellation)
        {
            await _service.CreateBranchOfferAsync(dto, cancellation);
            return Created();
        }
    }
}
