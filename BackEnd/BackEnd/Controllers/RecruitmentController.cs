using Application.VerticalSlice.RecrutmentPart.DTOs.Create;
using Application.VerticalSlice.RecrutmentPart.Services;
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
    }
}
