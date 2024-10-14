using Application.Features.Internship.InternshipPart.DTOs.Create;
using Application.Features.Internship.InternshipPart.DTOs.Update;
using Application.Features.Internship.InternshipPart.Services;
using Application.Features.Internship.RecrutmentPart.DTOs.Create;
using Application.Features.Internship.RecrutmentPart.DTOs.SetAnswerByCompany;
using Application.Features.Internship.RecrutmentPart.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InternshipController : ControllerBase
    {
        //Values
        private readonly IInternshipService _internshipService;
        private readonly IRecruitmentService _recruitmentService;


        //Constructor
        public InternshipController
            (
            IInternshipService internshipService,
            IRecruitmentService recruitmentService
            )
        {
            _internshipService = internshipService;
            _recruitmentService = recruitmentService;
        }


        //==================================================================================================
        //==================================================================================================
        //==================================================================================================
        //Public Methods

        //Recrutment Part
        [Authorize]
        [HttpPost("recruitment")]
        public async Task<IActionResult> CreateRecruitmentByPersonAsync
            (
            [Required] Guid branchId,
            [Required] Guid offerId,
            [Required] DateTime created,
            CreateRecruitmentRequestDto dto,
            CancellationToken cancellation
            )
        {
            var claims = User.Claims.ToList();
            var result = await _recruitmentService.CreateByPersonAsync
                (
                claims,
                branchId,
                offerId,
                created,
                dto,
                cancellation
                );
            return StatusCode(201, result);
        }

        [Authorize]
        [HttpPut("recruitment/answer")]
        public async Task<IActionResult> SetAnswerRecruitmentByCompanyAsync
            (
            [Required] Guid branchId,
            [Required] Guid offerId,
            [Required] DateTime created,
            [Required] Guid personId,
            SetAnswerByCompanyRecrutmentDto dto,
            CancellationToken cancellation
            )
        {
            var claims = User.Claims.ToList();
            var result = await _recruitmentService.SetAnswerByCompanyAsync
                (
                claims,
                branchId,
                offerId,
                created,
                personId,
                dto,
                cancellation
                );
            return StatusCode(200, result);
        }

        //Intership Part
        [Authorize]
        [HttpPost()]
        public async Task<IActionResult> CreateInternshipAsync
            (
            [Required] Guid branchId,
            [Required] Guid offerId,
            [Required] DateTime created,
            [Required] Guid personId,
            CreateInternshipRequestDto dto,
            CancellationToken cancellation
            )
        {
            var claims = User.Claims.ToList();
            var result = await _internshipService.CreateAsync
                (
                claims,
                branchId,
                offerId,
                created,
                personId,
                dto,
                cancellation
                );
            return StatusCode(201, result);
        }

        [Authorize]
        [HttpPut("{idInternship:guid}")]
        public async Task<IActionResult> UpdateInternshipAsync
            (
            Guid idInternship,
            UpdateInternshipRequestDto dto,
            CancellationToken cancellation
            )
        {
            var claims = User.Claims.ToList();
            var result = await _internshipService.UpdateAsync
                (
                claims,
                idInternship,
                dto,
                cancellation
                );
            return StatusCode(200, result);
        }

        //==================================================================================================
        //==================================================================================================
        //==================================================================================================
        //Private Methods
    }
}
