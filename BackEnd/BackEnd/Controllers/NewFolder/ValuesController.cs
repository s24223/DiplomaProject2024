using Application.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Controllers.NewFolder
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly DiplomaProjectContext _context;

        public ValuesController(DiplomaProjectContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var result = await _context.AdministrativeDivisions.Where(x => x.Name == "Warszawa").ToListAsync();
            return Ok(result);
        }
    }
}
