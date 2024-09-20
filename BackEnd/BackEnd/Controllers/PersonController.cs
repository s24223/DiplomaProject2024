using Application.Database.Models;
using Application.VerticalSlice.PersonPart.DTOs.CreateProfile;
using Application.VerticalSlice.PersonPart.Services;
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
            CreatePersonProfileRequestDto dto,
            CancellationToken cancellation
            )
        {
            var claims = User.Claims.ToList();
            await _personService.CreatePersonProfileAsync(claims, dto, cancellation);
            return Created();
        }
    }
}
