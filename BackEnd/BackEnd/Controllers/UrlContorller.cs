using Application.VerticalSlice.UrlPart.DTOs.Create;
using Application.VerticalSlice.UrlPart.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UrlContorller : ControllerBase
    {
        private readonly IUrlService _urlService;

        public UrlContorller(IUrlService urlService)
        {
            _urlService = urlService;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateUrlAsync(CreateUrlRequestDto dto,
            CancellationToken cancellation)
        {
            var claims = User.Claims.ToList();
            return Ok(await _urlService.CreateAsync(claims, dto, cancellation));
        }
    }
}
