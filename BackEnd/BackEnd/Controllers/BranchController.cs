using Application.Features.Company.BranchPart.DTOs.CreateProfile;
using Application.Features.Company.BranchPart.Services;
using Application.Features.Company.BranchPart.DTOs.UpdateProfile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BranchController : ControllerBase
    {
        private readonly IBranchService _service;

        public BranchController(IBranchService service)
        {
            _service = service;
        }
        [HttpPost]
        [HttpPost("create")]
        public async Task<IActionResult> CreateBranchAsync
            (
            CreateBranchProfileRequestDto dto,
            CancellationToken cancellation
            )
        {
            await _service.CreateBranchAsync(dto, cancellation);
            return Created();
        }
        [Authorize]
        [HttpPut("{id:guid}/update")]
        public async Task<IActionResult> UpdateBranchAsync
            (
           
            Guid id,
            UpdateBranchProfileRequestDto dto,
            CancellationToken cancellation
            )
        {
            // var claims = User.Claims.ToList();
            await _service.UpdateBranchAsync(id, dto, cancellation);
            return Created();
        }

    }
}
