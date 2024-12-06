using Application.Features.Addresses.Commands.DTOs.Create;
using Application.Features.Addresses.Commands.DTOs.Update;
using Application.Shared.DTOs.Response;

namespace Application.Features.Addresses.Commands.Services
{
    public interface IAddressCmdSvc
    {
        //DML
        Task<ResponseItem<CreateAddressResponseDto>> CreateAsync
            (
            CreateAddressRequestDto dto,
            CancellationToken cancellation
            );

        Task<Response> UpdateAsync
            (
            Guid id,
            UpdateAddressRequestDto dto,
            CancellationToken cancellation
            );

    }
}
