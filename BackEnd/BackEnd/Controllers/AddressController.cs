using Application.VerticalSlice.AddressPart.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        public IAddressSqlClientRepository _repository;
        public AddressController(IAddressSqlClientRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> aa
            (
            string administrativeDivisionName,
            string streetName,
            CancellationToken cancellation
            )
        {
            return Ok(await _repository.GetDivisionsStreetsAsync
                (administrativeDivisionName, streetName, cancellation));
        }
    }
}
