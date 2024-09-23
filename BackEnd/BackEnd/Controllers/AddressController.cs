using Application.VerticalSlice.AddressPart.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        public IAddressRepository _repository;
        public AddressController(IAddressRepository repository)
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
