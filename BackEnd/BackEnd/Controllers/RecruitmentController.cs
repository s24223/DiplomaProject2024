using Application.Features.Internship.RecrutmentPart.DTOs;
using Application.Features.Internship.RecrutmentPart.DTOs.Create;
using Application.Features.Internship.RecrutmentPart.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecruitmentController : ControllerBase
    {
        private readonly IRecruitmentService _recruitmentService;
        public RecruitmentController(IRecruitmentService recruitmentService)
        {
            _recruitmentService = recruitmentService;
        }

        [Authorize]
        [HttpPost("created")]
        public async Task<IActionResult> CreateRecruitmentAsync
            (
            CreateRecruitmentRequestDto dto,
            CancellationToken cancellation
            )
        {
            var claims = User.Claims.ToList();
            await _recruitmentService.CreateAsync(claims, dto, cancellation);
            return Created();
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateAsync(
            UpdateRecrutmentDto dto,
            CancellationToken cancellation)
        {
            var claims = User.Claims.ToList();
            return Ok(await _recruitmentService.UpdateRecruitmentAsync(claims, dto, cancellation));
        }
    }
}
