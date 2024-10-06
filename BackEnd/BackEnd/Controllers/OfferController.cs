using Application.Features.Company.OfferPart.DTOs.Create;
using Application.Features.Company.OfferPart.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfferController : ControllerBase
    {
        private readonly IOfferService _offerService;

        public OfferController(IOfferService offerService)
        {
            _offerService = offerService;
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateOfferAsync
            (
            CreateOfferRequestDto dto,
            CancellationToken cancellation
            )
        {
           // var claims = User.Claims.ToList();
            await _offerService.CreateOfferProfileAsync(/*claims,*/ dto, cancellation);
            return Created();
        }
    }
}
