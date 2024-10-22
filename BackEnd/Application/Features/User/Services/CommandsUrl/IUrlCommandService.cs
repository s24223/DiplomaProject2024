using Application.Features.User.DTOs.CommandsUrl.Create;
using Application.Features.User.DTOs.CommandsUrl.Delete;
using Application.Features.User.DTOs.CommandsUrl.Update;
using Application.Shared.DTOs.Response;
using System.Security.Claims;


namespace Application.Features.User.Services.CommandsUrl
{
    public interface IUrlCommandService
    {
        Task<Response> CreateAsync
            (
            IEnumerable<Claim> claims,
            IEnumerable<CreateUrlRequestDto> dtos,
            CancellationToken cancellation
            );

        Task<Response> UpdateAsync
           (
           IEnumerable<Claim> claims,
           IEnumerable<UpdateUrlRequestDto> dtos,
           CancellationToken cancellation
           );

        Task<Response> DeleteAsync
            (
            IEnumerable<Claim> claims,
            IEnumerable<DeleteUrlRequestDto> dtos,
            CancellationToken cancellation
            );

    }
}
