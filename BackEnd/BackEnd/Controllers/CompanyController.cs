using Application.Features.Companies.DTOs.CommandsBranchOffer.CommandsBranchOffer.CreateBranchOffer;
using Application.Features.Companies.DTOs.CommandsBranchOffer.CommandsBranchOffer.UpdateBranchOffer;
using Application.Features.Companies.DTOs.CommandsBranchOffer.CommandsOffer.CreateOffer;
using Application.Features.Companies.DTOs.CommandsBranchOffer.CommandsOffer.UpdateOffer;
using Application.Features.Companies.DTOs.CommandsCompanyBranch.CommandsBranch.Create;
using Application.Features.Companies.DTOs.CommandsCompanyBranch.CommandsBranch.Update;
using Application.Features.Companies.DTOs.CommandsCompanyBranch.CommandsCompany.Create;
using Application.Features.Companies.DTOs.CommandsCompanyBranch.CommandsCompany.Update;
using Application.Features.Companies.Services.CommandsBranchOffer;
using Application.Features.Companies.Services.CommandsCompanyBranch;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        //Values
        private readonly ICompanyBranchCommandService _companyBranchService;
        private readonly IBranchOfferService _branchOfferService;


        //Constrructor
        public CompanyController
            (
            ICompanyBranchCommandService companyBranchService,
            IBranchOfferService branchOfferService
            )
        {
            _companyBranchService = companyBranchService;
            _branchOfferService = branchOfferService;
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
            var result = await _companyBranchService.CreateAsync(claims, dto, cancellation);
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
            var result = await _companyBranchService.UpdateCompanyAsync(claims, dto, cancellation);
            return StatusCode(200, result);
        }


        //Branch Part
        //DML
        [Authorize]
        [HttpPost("branches")]
        public async Task<IActionResult> CreateBranchAsync
            (
            IEnumerable<CreateBranchRequestDto> dtos,
            CancellationToken cancellation
            )
        {
            var claims = User.Claims.ToList();
            var result = await _companyBranchService.CreateBranchesAsync(claims, dtos, cancellation);
            return StatusCode(201, result);
        }

        [Authorize]
        [HttpPut("branches")]
        public async Task<IActionResult> UpdateBranchAsync
            (
            IEnumerable<UpdateBranchRequestDto> dtos,
            CancellationToken cancellation
            )
        {
            var claims = User.Claims.ToList();
            var result = await _companyBranchService.UpdateBranchesAsync(claims, dtos, cancellation);
            return StatusCode(200, result);
        }

        //Offer Part
        //DML
        [Authorize]
        [HttpPost("offers")]
        public async Task<IActionResult> CreateOffersAsync
           (
           IEnumerable<CreateOfferRequestDto> dtos,
           CancellationToken cancellation
           )
        {
            var claims = User.Claims.ToList();
            var result = await _branchOfferService.CreateOffersAsync(claims, dtos, cancellation);
            return StatusCode(201, result);
        }

        [Authorize]
        [HttpPut("offers")]
        public async Task<IActionResult> UpdateOffersAsync
            (
            IEnumerable<UpdateOfferRequestDto> dtos,
            CancellationToken cancellation
            )
        {
            var claims = User.Claims.ToList();
            var result = await _branchOfferService.UpdateOffersAsync(claims, dtos, cancellation);
            return StatusCode(200, result);
        }
        //DQL

        //OfferBranch Part
        //DML
        [Authorize]
        [HttpPost("BranchOfferConnection")]
        public async Task<IActionResult> CreateBranchOfferConnectionsAsync
           (
           IEnumerable<CreateBranchOfferRequestDto> dtos,
           CancellationToken cancellation
           )
        {
            var claims = User.Claims.ToList();
            var result = await _branchOfferService.CreateBranchOfferAsync
                (
                claims,
                dtos,
                cancellation
                );
            return StatusCode(201, result);
        }

        [Authorize]
        [HttpPut("BranchOfferConnection")]
        public async Task<IActionResult> UpdateBranchOfferConnectionsAsync
            (
            IEnumerable<UpdateBranchOfferRequestDto> dtos,
            CancellationToken cancellation
            )
        {
            var claims = User.Claims.ToList();
            var result = await _branchOfferService.UpdateBranchOfferAsync
                (
                claims,
                dtos,
                cancellation
                );
            return StatusCode(200, result);
        }
        //DQL
    }
}
