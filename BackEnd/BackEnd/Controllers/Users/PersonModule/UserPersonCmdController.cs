using Application.Features.Persons.Commands.DTOs;
using Application.Features.Persons.Commands.Services;
using Application.Shared.DTOs.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers.Users.PersonModule
{
    [Route("api/User/person")]
    [ApiController]
    public class UserPersonCmdController : ControllerBase
    {
        //values
        private readonly IPersonCmdSvc _personService;


        //Constructor
        public UserPersonCmdController(IPersonCmdSvc personService)
        {
            _personService = personService;
        }


        //============================================================================================
        //============================================================================================
        //============================================================================================
        //Public Methods
        [Authorize]
        [HttpPost()]
        public async Task<IActionResult> CreateAsync
            (
            CreatePersonReq dto,
            CancellationToken cancellation
            )
        {
            var claims = User.Claims.ToList();
            var result = await _personService.CreateAsync(claims, dto, cancellation);
            if (result.Status == EnumResponseStatus.UserFault)
            {
                return StatusCode(400, result);
            }
            return StatusCode(201, result);
        }

        [Authorize]
        [HttpPut()]
        public async Task<IActionResult> UpdateAsync
            (
            UpdatePersonReq dto,
            CancellationToken cancellation
            )
        {
            var claims = User.Claims.ToList();
            var result = await _personService.UpdateAsync(claims, dto, cancellation);
            if (result.Status == EnumResponseStatus.UserFault)
            {
                return StatusCode(400, result);
            }
            return StatusCode(200, result);
        }

        //============================================================================================
        //============================================================================================
        //============================================================================================
        //Private Methods
    }
}
