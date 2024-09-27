using Application.Shared.DTOs.Response;
using Application.VerticalSlice.UrlPart.DTOs;
using Application.VerticalSlice.UrlPart.DTOs.Create;
using System.Security.Claims;


namespace Application.VerticalSlice.UrlPart.Services
{
    public interface IUrlService
    {
        Task<Response> CreateAsync
            (
            IEnumerable<Claim> claims,
            CreateUrlRequestDto dto,
            CancellationToken cancellation
            );

        IEnumerable<UrlTypeResponseDto> GetUrlTypes();
    }
}
