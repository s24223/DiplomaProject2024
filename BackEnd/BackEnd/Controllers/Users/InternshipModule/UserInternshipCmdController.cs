using Application.Features.Internships.Commands.Internships.DTOs;
using Application.Features.Internships.Commands.Internships.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BackEnd.Controllers.Users.InternshipModule
{
    [Route("api/User/internship")]
    [ApiController]
    public class UserInternshipCmdController : ControllerBase
    {
        //values
        private readonly IInternshipCmdSvc _internshipService;


        //Constructor 
        public UserInternshipCmdController(IInternshipCmdSvc internshipService)
        {
            _internshipService = internshipService;
        }


        //============================================================================================
        //============================================================================================
        //============================================================================================
        //Public Methods
        [Authorize]
        [HttpPost()]
        public async Task<IActionResult> CreateInternshipAsync
        (
            [Required] Guid recrutmentId,
            CreateInternshipReq dto,
            CancellationToken cancellation
            )
        {
            var claims = User.Claims.ToList();
            var result = await _internshipService.CreateAsync
                (
                claims,
                recrutmentId,
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
            UpdateInternshipReq dto,
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
        //============================================================================================
        //============================================================================================
        //============================================================================================
        //Private Methods
    }
}
