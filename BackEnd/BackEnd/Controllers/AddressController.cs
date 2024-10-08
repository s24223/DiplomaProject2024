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
        public IAddressService _service;
        public AddressController
            (
            IAddressService service
            )
        {
            _service = service;
        }


        [HttpGet()]
        public async Task<IActionResult> GetCollocationsAsync
            (
            string divisionName,
            string streetName,
            CancellationToken cancellation
            )
        {
            return Ok(await _service.GetCollocationsAsync(divisionName, streetName, cancellation));
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetAddressAsync
            (
            Guid id,
            CancellationToken cancellation
            )
        {
            return Ok(await _service.GetAddressAsync(id, cancellation));
        }

        [HttpPost()]
        public async Task<IActionResult> CreateAsync
            (
            CreateAddressRequestDto dto,
            CancellationToken cancellation
            )
        {
            return Ok(await _service.CreateAsync(dto, cancellation));
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateAsync
            (
            Guid id,
            UpdateAddressRequestDto dto,
            CancellationToken cancellation
            )
        {
            return Ok(await _service.UpdateAsync(id, dto, cancellation));
        }
    }
}
