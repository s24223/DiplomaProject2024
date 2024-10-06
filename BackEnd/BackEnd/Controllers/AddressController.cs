using Application.Features.Address.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        public IAddressRepository _repository;
        public AddressController
            (
            IAddressRepository repository
            )
        {
            _repository = repository;
        }


        [HttpGet()]
        public async Task<IActionResult> aaz
            (
            string divisionName,
            string streetName,
            CancellationToken cancellation
            )
        {
            return Ok(await _repository.GetCollocationsAsync(divisionName, streetName, cancellation));
        }
    }
}
