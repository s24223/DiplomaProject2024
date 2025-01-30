using Application.Features.Internships.Commands.Recrutments.DTOs;
using Application.Features.Internships.Commands.Recrutments.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BackEnd.Controllers.Users.InternshipModule
{
    [Route("api/User/recruitment")]
    [ApiController]
    public class UserRecruitmentCmdController : ControllerBase
    {
        //values
        private readonly IRecruitmentCmdSvc _recruitmentService;


        //Constructor
        public UserRecruitmentCmdController(IRecruitmentCmdSvc cmdSvc)
        {
            _recruitmentService = cmdSvc;
        }


        //=============================================================================================
        //=============================================================================================
        //=============================================================================================
        //Public Methods

        [Authorize]
        [HttpPost()] //For Person
        public async Task<IActionResult> CreateRecruitmentByPersonAsync
            (
            [Required] Guid branchOfferId,
            CreateRecruitmentReq dto,
            CancellationToken cancellation
            )
        {
            var claims = User.Claims.ToList();

            if (dto.File != null && dto.File.ContentType != "application/pdf")
            {
                return BadRequest("Przesłany plik nie jest plikiem PDF.");
            }

            var result = await _recruitmentService.CreateByPersonAsync(
                claims,
                branchOfferId,
                dto,
                cancellation
                );
            return StatusCode(201, result);
        }


        [Authorize]
        [HttpPut("{recruitmentId:guid}/answer")] //For Company
        public async Task<IActionResult> SetAnswerRecruitmentByCompanyAsync
            (
            [Required] Guid recruitmentId,
            SetAnswerReq dto,
            CancellationToken cancellation
            )
        {
            var claims = User.Claims.ToList();
            var result = await _recruitmentService.SetAnswerByCompanyAsync
                (
                claims,
                recruitmentId,
                dto,
                cancellation
                );
            return StatusCode(200, result);
        }
        //=============================================================================================
        //=============================================================================================
        //=============================================================================================
        //Public Methods
    }
}
