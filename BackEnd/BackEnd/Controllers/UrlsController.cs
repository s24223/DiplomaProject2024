using Application.VerticalSlice.UrlPart.DTOs.Create;
using Application.VerticalSlice.UrlPart.DTOs.Update;
using Application.VerticalSlice.UrlPart.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UrlsController : ControllerBase
    {
        private readonly IUrlService _urlService;

        public UrlsController(IUrlService urlService)
        {
            _urlService = urlService;
        }


        //===================================================================================================
        //DML
        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateAsync
            (
            CreateUrlRequestDto dto,
            CancellationToken cancellation
            )
        {
            var claims = User.Claims.ToList();
            return Ok(await _urlService.CreateAsync(claims, dto, cancellation));
        }

        [Authorize]
        [HttpPut("{urlTypeId:int}&{created:datetime}")]
        public async Task<IActionResult> UpdateAsync
            (
            int urlTypeId,
            DateTime created,
            UpdateUrlRequestDto dto,
            CancellationToken cancellation
            )
        {
            var claims = User.Claims.ToList();
            return Ok(await _urlService.UpdateAsync(claims, urlTypeId, created, dto, cancellation));
        }

        [Authorize]
        [HttpDelete("{urlTypeId:int}&{created:datetime}")]
        public async Task<IActionResult> DeleteAsync
            (
            int urlTypeId,
            DateTime created,
            CancellationToken cancellation
            )
        {
            var claims = User.Claims.ToList();
            return Ok(await _urlService.DeleteAsync(claims, urlTypeId, created, cancellation));
        }
        //===================================================================================================
        //DQL
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUrlsAsync(CancellationToken cancellation)
        {
            var claims = User.Claims.ToList();
            return Ok(await _urlService.GetUrlsAsync(claims, cancellation));
        }


        [Authorize]
        [HttpGet("types")]
        public IActionResult GetUrlTypes()
        {
            return StatusCode(200, _urlService.GetUrlTypes());
        }

    }
}
