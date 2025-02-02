﻿using Application.Features.Addresses.Commands.DTOs;
using Application.Shared.DTOs.Response;

namespace Application.Features.Addresses.Commands.Services
{
    public interface IAddressCmdSvc
    {
        Task<ResponseItem<CreateAddressResp>> CreateAsync(
            CreateAddressReq dto,
            CancellationToken cancellation);
    }
}
