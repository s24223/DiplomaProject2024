using Application.Features.User.UrlPart.DTOs;
using Application.Features.User.UrlPart.DTOs.Create;
using Application.Features.User.UrlPart.DTOs.Update;
using Application.Shared.DTOs.Response;
using System.Security.Claims;


namespace Application.Features.User.UrlPart.Services
{
    public interface IUrlService
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
