using Application.Features.Users.Commands.Urls.DTOs.Create;
using Application.Features.Users.Commands.Urls.DTOs.Delete;
using Application.Features.Users.Commands.Urls.DTOs.Update;
using Application.Shared.DTOs.Response;
using System.Security.Claims;


namespace Application.Features.Users.Commands.Urls.Services
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
