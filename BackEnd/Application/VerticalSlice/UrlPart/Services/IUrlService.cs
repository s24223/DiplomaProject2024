using Application.Shared.DTOs.Response;
using Application.VerticalSlice.UrlPart.DTOs;
using Application.VerticalSlice.UrlPart.DTOs.Create;
using Application.VerticalSlice.UrlPart.DTOs.Update;
using System.Security.Claims;


namespace Application.VerticalSlice.UrlPart.Services
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
        Task<IEnumerable<UrlResponseDto>> GetUrlsAsync
            (
            IEnumerable<Claim> claims,
            CancellationToken cancellation
            );

        IEnumerable<UrlTypeResponseDto> GetUrlTypes();
    }
}
