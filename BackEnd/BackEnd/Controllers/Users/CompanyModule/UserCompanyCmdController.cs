using Application.Features.Companies.Commands.BranchOffers.DTOs.CommandsBranchOffer.CreateBranchOffer.Request;
using Application.Features.Companies.Commands.BranchOffers.DTOs.CommandsBranchOffer.UpdateBranchOffer.Request;
using Application.Features.Companies.Commands.BranchOffers.DTOs.CommandsOffer.CreateOffer.Request;
using Application.Features.Companies.Commands.BranchOffers.DTOs.CommandsOffer.UpdateOffer.Request;
using Application.Features.Companies.Commands.BranchOffers.Services;
using Application.Features.Companies.Commands.CompanyBranches.DTOs.CommandsBranch.Create;
using Application.Features.Companies.Commands.CompanyBranches.DTOs.CommandsBranch.Update;
using Application.Features.Companies.Commands.CompanyBranches.DTOs.CommandsCompany.Create;
using Application.Features.Companies.Commands.CompanyBranches.DTOs.CommandsCompany.Update;
using Application.Features.Companies.Commands.CompanyBranches.Services;
using Application.Shared.DTOs.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers.Users.CompanyModule
{
    [Route("api/User/company")]
    [ApiController]
    public class UserCompanyCmdController : ControllerBase
    {
        //Values
        private readonly ICompanyBranchCmdSvc _companyBranchService;
        private readonly IBranchOfferCommandService _branchOfferService;

        //Controller
        public UserCompanyCmdController(
            ICompanyBranchCmdSvc companyBranchService,
            IBranchOfferCommandService branchOfferService)
        {
            _companyBranchService = companyBranchService;
            _branchOfferService = branchOfferService;
        }


        //============================================================================================
        //============================================================================================
        //============================================================================================
        //Public Methods
        [Authorize]
        [HttpPost()]
        public async Task<IActionResult> CreateCompanyAsync
            (
            CreateCompanyReq dto,
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
            UpdateCompanyReq dto,
            CancellationToken cancellation
            )
        {
            var claims = User.Claims.ToList();
            var result = await _companyBranchService.UpdateCompanyAsync(claims, dto, cancellation);
            return StatusCode(200, result);
        }

        //================================================================================================================
        [Authorize]
        [HttpPost("branches")]
        public async Task<IActionResult> CreateBranchAsync
            (
            IEnumerable<CreateBranchReq> dtos,
            CancellationToken cancellation
            )
        {
            var claims = User.Claims.ToList();
            var result = await _companyBranchService.CreateBranchesAsync(claims, dtos, cancellation);
            if (result.Status == EnumResponseStatus.UserFault)
            {
                return StatusCode(400, result);
            }
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
            if (result.Status == EnumResponseStatus.UserFault)
            {
                return StatusCode(400, result);
            }
            return StatusCode(200, result);
        }

        //================================================================================================================
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
            if (result.Status == EnumResponseStatus.UserFault)
            {
                return StatusCode(400, result);
            }
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
            if (result.Status == EnumResponseStatus.UserFault)
            {
                return StatusCode(400, result);
            }
            return StatusCode(200, result);
        }

        [Authorize]
        [HttpPost("branches&offers")]
        public async Task<IActionResult> CreateBranchOfferConnectionsAsync
           (
           IEnumerable<CreateBranchOfferRequestDto> dtos,
           CancellationToken cancellation
           )
        {
            var claims = User.Claims.ToList();
            var result = await _branchOfferService.CreateBranchOffersAsync
                (
                claims,
                dtos,
                cancellation
                );
            return StatusCode(201, result);
        }

        [Authorize]
        [HttpPut("branches&offers")]
        public async Task<IActionResult> UpdateBranchOfferConnectionsAsync
            (
            IEnumerable<UpdateBranchOfferRequestDto> dtos,
            CancellationToken cancellation
            )
        {
            var claims = User.Claims.ToList();
            var result = await _branchOfferService.UpdateBranchOffersAsync
                (
                claims,
                dtos,
                cancellation
                );
            return StatusCode(200, result);
        }

        //============================================================================================
        //============================================================================================
        //============================================================================================
        //Private Methods
    }
}
