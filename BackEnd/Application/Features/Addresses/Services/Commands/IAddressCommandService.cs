using Application.Features.Addresses.DTOs.Commands.Create;
using Application.Features.Addresses.DTOs.Commands.Update;
using Application.Shared.DTOs.Response;

namespace Application.Features.Addresses.Services.Commands
{
    public interface IAddressCommandService
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
