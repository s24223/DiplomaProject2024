using Application.Features.Person.DTOs.Create;
using Application.Features.Person.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly IPersonService _personService;

        public PersonController(IPersonService personService)
        {
            _personService = personService;
        }
        [Authorize]
        [HttpPost("created")]
        public async Task<IActionResult> CreatePersonAsync
            (
            CreatePersonRequestDto dto,
            CancellationToken cancellation
            )
        {
            var claims = User.Claims.ToList();
            await _personService.CreatePersonProfileAsync(claims, dto, cancellation);
            return Created();
        }
    }
}
