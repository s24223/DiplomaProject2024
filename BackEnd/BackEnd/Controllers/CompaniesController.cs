﻿using Application.Features.Companies.DTOs.CommandsBranchOffer.CreateBranchOffer;
using Application.Features.Companies.DTOs.CommandsBranchOffer.CreateOffer;
using Application.Features.Companies.DTOs.CommandsBranchOffer.UpdateBranchOffer;
using Application.Features.Companies.DTOs.CommandsBranchOffer.UpdateOffer;
using Application.Features.Companies.DTOs.CommandsCompanyBranch.B.Create;
using Application.Features.Companies.DTOs.CommandsCompanyBranch.B.Update;
using Application.Features.Companies.DTOs.CommandsCompanyBranch.C.Create;
using Application.Features.Companies.DTOs.CommandsCompanyBranch.C.Update;
using Application.Features.Companies.Services.CommandsBranchOffer;
using Application.Features.Companies.Services.CommandsCompanyBranch;
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
        private readonly IBranchOfferService _branchOfferService;


        //Constrructor
        public CompaniesController
            (
            IBranchService branchService,
            ICompanyService companyService,
            IBranchOfferService branchOfferService
            )
        {
            _branchService = branchService;
            _companyService = companyService;
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

        //Offer Part
        //DML
        [Authorize]
        [HttpPost("offers")]
        public async Task<IActionResult> CreateOfferAsync
           (
           CreateOfferRequestDto dto,
           CancellationToken cancellation
           )
        {
            var claims = User.Claims.ToList();
            var result = await _branchOfferService.CreateOfferAsync(claims, dto, cancellation);
            return StatusCode(201, result);
        }

        [Authorize]
        [HttpPut("offers/{offerId:guid}")]
        public async Task<IActionResult> UpdateOfferAsync
            (
            Guid offerId,
            UpdateOfferRequestDto dto,
            CancellationToken cancellation
            )
        {
            var claims = User.Claims.ToList();
            var result = await _branchOfferService.UpdateOfferAsync(claims, offerId, dto, cancellation);
            return StatusCode(200, result);
        }
        //DQL

        //OfferBranch Part
        //DML
        [Authorize]
        [HttpPost("branch/{branchId:guid}/offers/{offerId:guid}")]
        public async Task<IActionResult> CreateBranchOfferConnectionAsync
           (
           Guid branchId,
           Guid offerId,
           CreateBranchOfferRequestDto dto,
           CancellationToken cancellation
           )
        {
            var claims = User.Claims.ToList();
            var result = await _branchOfferService.CreateBranchOfferConnectionAsync
                (
                claims,
                branchId,
                offerId,
                dto,
                cancellation
                );
            return StatusCode(201, result);
        }

        [Authorize]
        [HttpPut("branch/{branchId:guid}/offers/{offerId:guid}&{created}")]
        public async Task<IActionResult> UpdateBranchOfferConnectionAsync
            (
            Guid branchofferId,
            UpdateBranchOfferRequestDto dto,
            CancellationToken cancellation
            )
        {
            var claims = User.Claims.ToList();
            var result = await _branchOfferService.UpdateBranchOfferConnectionAsync
                (
                claims,
                branchofferId,
                dto,
                cancellation
                );
            return StatusCode(200, result);
        }
        //DQL
    }
}
