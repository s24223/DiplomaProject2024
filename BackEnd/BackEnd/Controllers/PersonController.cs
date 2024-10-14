using Application.Features.Person.DTOs.Create;
using Application.Features.Person.DTOs.Update;
using Application.Features.Person.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        //Values
        private readonly IPersonService _personService;


        //Cosntructor
        public PersonController(IPersonService personService)
        {
            _personService = personService;
        }


        //=================================================================================
        //=================================================================================
        //=================================================================================
        //Public Methods
        //DML

        [Authorize]
        [HttpPost()]
        public async Task<IActionResult> CreateAsync
            (
            CreatePersonRequestDto dto,
            CancellationToken cancellation
            )
        {
            var claims = User.Claims.ToList();
            var result = await _personService.CreateAsync(claims, dto, cancellation);
            return StatusCode(201, result);
        }

        [Authorize]
        [HttpPut()]
        public async Task<IActionResult> UpdateAsync
            (
            UpdatePersonRequestDto dto,
            CancellationToken cancellation
            )
        {
            var claims = User.Claims.ToList();
            var result = await _personService.UpdateAsync(claims, dto, cancellation);
            return StatusCode(201, result);
        }

        //=================================================================================
        //=================================================================================
        //=================================================================================
        //Private Methods
    }
}
