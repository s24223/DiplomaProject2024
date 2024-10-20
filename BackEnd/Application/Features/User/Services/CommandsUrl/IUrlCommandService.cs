using Application.Features.User.DTOs.CommandsUrl;
using Application.Features.User.DTOs.CommandsUrl.Create;
using Application.Features.User.DTOs.CommandsUrl.Update;
using Application.Shared.DTOs.Response;
using System.Security.Claims;


namespace Application.Features.User.Services.CommandsUrl
{
    public interface IUrlCommandService
    {
        //====================================================================================================
        //DML
        Task<Response> CreateAsync
            (
            IEnumerable<Claim> claims,
            CreateUrlRequestDto dto,
            CancellationToken cancellation
            );

        Task<Response> UpdateAsync
           (
           IEnumerable<Claim> claims,
           int urlTypeId,
           DateTime created,
           UpdateUrlRequestDto dto,
           CancellationToken cancellation
           );

        Task<Response> DeleteAsync
            (
            IEnumerable<Claim> claims,
            int urlTypeId,
            DateTime created,
            CancellationToken cancellation
            );

        //====================================================================================================
        //DQL
        IEnumerable<UrlTypeResponseDto> GetUrlTypes();
    }
}
