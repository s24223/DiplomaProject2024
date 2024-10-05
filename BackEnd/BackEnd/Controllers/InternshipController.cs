﻿using Application.VerticalSlice.InternshipPart.DTOs;
using Application.VerticalSlice.InternshipPart.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InternshipController : ControllerBase
    {
        private readonly IInternshipService _serivce;

        public InternshipController(
            IInternshipService serivce)
        {
            _serivce = serivce;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateInternshipAsync(CreateInternshipDto dto,
            CancellationToken cancellation)
        {
            var claims = User.Claims.ToList();
            return Ok(await _serivce.CreateInternshipAsync(
                claims, dto, cancellation));
        }
    }
}