﻿using Application.VerticalSlice.CompanyPart.DTOs.CreateProfile;
using Application.VerticalSlice.PersonPart.DTOs.CreateProfile;
using System.Security.Claims;


namespace Application.VerticalSlice.PersonPart.Services
{
    public interface IPersonService
    {
        Task CreatePersonProfileAsync
            (
            IEnumerable<Claim> claims,
            CreatePersonProfileRequestDto dto,
            CancellationToken cancellation
            );
    }
}