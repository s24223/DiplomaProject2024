using Application.Features.Companies.CompanyPart.DTOs.Create;
using Application.Features.Companies.CompanyPart.DTOs.Update;
using Application.Features.Companies.CompanyPart.Services;
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
            CreateCompanyRequestDto dto,
            CancellationToken cancellation
            )
        {
            var claims = User.Claims.ToList();
            await _companyService.CreateAsync(claims, dto, cancellation);
            return Created();
        }

        [Authorize]
        [HttpPut("update")]
        public async Task<IActionResult> UpdateCompanyAsync
            (
            UpdateCompanyRequestDto dto,
            CancellationToken cancellation
            )
        {
            var claims = User.Claims.ToList();
            await _companyService.UpdateAsync(claims, dto, cancellation);
            return Created();
        }
    }
}
