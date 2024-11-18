using Application.Features.Persons.Commands.DTOs.Create;
using Application.Features.Persons.Commands.DTOs.Update;
using Application.Shared.DTOs.Features.Persons;
using Application.Shared.DTOs.Response;
using System.Security.Claims;


namespace Application.Features.Persons.Commands.Services
{
    public interface IPersonCmdService
    {
        Task<ResponseItem<PersonResponseDto>> CreateAsync
            (
            IEnumerable<Claim> claims,
            CreatePersonRequestDto dto,
            CancellationToken cancellation
            );

        Task<ResponseItem<PersonResponseDto>> UpdateAsync
            (
            IEnumerable<Claim> claims,
            UpdatePersonRequestDto dto,
            CancellationToken cancellation
            );

    }
}
