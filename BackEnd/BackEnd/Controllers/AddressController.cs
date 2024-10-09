using Application.Features.Addresses.DTOs.Create;
using Application.Features.Addresses.DTOs.Update;
using Application.Features.Addresses.Services;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        //Values
        public IAddressService _service;


        //Controllers
        public AddressController
            (
            IAddressService service
            )
        {
            _service = service;
        }

        //==================================================================================================
        //==================================================================================================
        //==================================================================================================
        //Public Methods
        //DML
        [HttpPost()]
        public async Task<IActionResult> CreateAsync
            (
            CreateAddressRequestDto dto,
            CancellationToken cancellation
            )
        {
            var result = await _service.CreateAsync(dto, cancellation);
            return StatusCode(201, result);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateAsync
            (
            Guid id,
            UpdateAddressRequestDto dto,
            CancellationToken cancellation
            )
        {
            var result = await _service.UpdateAsync(id, dto, cancellation);
            return StatusCode(200, result);
        }

        //DQL
        [HttpGet("collocations")]
        public async Task<IActionResult> GetCollocationsAsync
            (
            string divisionName,
            string streetName,
            CancellationToken cancellation
            )
        {
            var result = await _service.GetCollocationsAsync(divisionName, streetName, cancellation);
            if (result.Count > 0)
            {
                return StatusCode(200, result);
            }
            else
            {
                return StatusCode(404);

            }
        }

        [HttpGet("divisionsDown")]
        public async Task<IActionResult> GetDivisionsDownAsync
            (
            int? id,
            CancellationToken cancellation
            )
        {
            var result = await _service.GetDivisionsDownAsync(id, cancellation);
            if (result.Count > 0)
            {
                return StatusCode(200, result);
            }
            else
            {
                return StatusCode(404);

            }
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetAddressAsync
            (
            Guid id,
            CancellationToken cancellation
            )
        {
            var result = await _service.GetAddressAsync(id, cancellation);
            return StatusCode(200, result);
        }
    }
}
