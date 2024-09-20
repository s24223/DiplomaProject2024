using Application.VerticalSlice.CompanyPart.DTOs.CreateProfile;
using Application.VerticalSlice.CompanyPart.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyService _companyService;

        public CompaniesController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateCompanyAsync
            (
            CreateCompanyProfileRequestDto dto,
            CancellationToken cancellation
            )
        {
            var claims = User.Claims.ToList();
            await _companyService.CreateCompanyProfileAsync(claims, dto, cancellation);
            return Created();
        }
    }
}
