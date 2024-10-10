using Application.Features.Companies.BranchPart.DTOs.Create;
using Application.Features.Companies.BranchPart.DTOs.Update;
using Application.Features.Companies.BranchPart.Services;
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
        //Values
        private readonly IBranchService _branchService;
        private readonly ICompanyService _companyService;



        //Constrructor
        public CompaniesController
            (
            IBranchService branchService,
            ICompanyService companyService
            )
        {
            _branchService = branchService;
            _companyService = companyService;
        }


        //==================================================================================================
        //==================================================================================================
        //==================================================================================================
        //Public Methods


        //Company Part
        //DML
        [Authorize]
        [HttpPost()]
        public async Task<IActionResult> CreateCompanyAsync
            (
            CreateCompanyRequestDto dto,
            CancellationToken cancellation
            )
        {
            var claims = User.Claims.ToList();
            var result = await _companyService.CreateAsync(claims, dto, cancellation);
            return StatusCode(201, result);
        }

        [Authorize]
        [HttpPut()]
        public async Task<IActionResult> UpdateCompanyAsync
            (
            UpdateCompanyRequestDto dto,
            CancellationToken cancellation
            )
        {
            var claims = User.Claims.ToList();
            var result = await _companyService.UpdateAsync(claims, dto, cancellation);
            return StatusCode(200, result);
        }


        //Branch Part
        //DML
        [Authorize]
        [HttpPost("branches")]
        public async Task<IActionResult> CreateBranchAsync
            (
            CreateBranchRequestDto dto,
            CancellationToken cancellation
            )
        {
            var claims = User.Claims.ToList();
            var result = await _branchService.CreateAsync(claims, dto, cancellation);
            return StatusCode(201, result);
        }

        [Authorize]
        [HttpPut("branches/{branchId:guid}")]
        public async Task<IActionResult> UpdateBranchAsync
            (
            Guid branchId,
            UpdateBranchRequestDto dto,
            CancellationToken cancellation
            )
        {
            var claims = User.Claims.ToList();
            var result = await _branchService.UpdateAsync(claims, branchId, dto, cancellation);
            return StatusCode(200, result);
        }


    }
}
