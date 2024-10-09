using Application.Features.Companies.BranchOfferPart.DTOs.CreateProfile;
using Application.Features.Companies.BranchOfferPart.Services;
using Microsoft.AspNetCore.Mvc;

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
