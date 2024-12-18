﻿using Application.Features.Addresses.Commands.DTOs;
using Application.Features.Addresses.Commands.Services;
using Application.Features.Addresses.Queries.Services;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        //Values
        private readonly IAddressCmdSvc _commandService;
        private readonly IAddressQuerySvc _queryService;


        //Controllers
        public AddressController(
            IAddressCmdSvc commandService,
            IAddressQuerySvc queryService)
        {
            _commandService = commandService;
            _queryService = queryService;
        }

        //==================================================================================================
        //==================================================================================================
        //==================================================================================================
        //Public Methods
        //DML
        [HttpPost()]
        public async Task<IActionResult> CreateAsync
            (
            CreateAddressReq dto,
            CancellationToken cancellation
            )
        {
            var result = await _commandService.CreateAsync(dto, cancellation);
            return StatusCode(201, result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetAddressAsync(
                    Guid id,
                    CancellationToken cancellation)
        {
            var result = await _queryService.GetAddressAsync(id, cancellation);
            return StatusCode(200, result);
        }
        /*
                [HttpPut("{id:guid}")]
                public async Task<IActionResult> UpdateAsync
                    (
                    Guid id,
                    UpdateAddressRequestDto dto,
                    CancellationToken cancellation
                    )
                {
                    var result = await _commandService.UpdateAsync(id, dto, cancellation);
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
                    var result = await _queryService.GetCollocationsAsync(divisionName, streetName, cancellation);
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
                    var result = await _queryService.GetDivisionsDownAsync(id, cancellation);
                    return StatusCode(200, result);

                }

                [HttpGet("divisionsDown2")]
                public async Task<IActionResult> GetDivisionsDown2Async
                    (
                    int? id,
                    CancellationToken cancellation
                    )
                {
                    var result = await _queryService.GetDivisionsDownHorizontalAsync(id, cancellation);
                    return StatusCode(200, result);
                }                

                [HttpGet("streets/{divisionId:int}")]
                public async Task<IActionResult> GetStreetsAsync
                    (
                    int divisionId,
                    CancellationToken cancellation
                    )
                {
                    var result = await _queryService.GetStreetsAsync(divisionId, cancellation);
                    return StatusCode(200, result);
                }*/
    }
}
