using Application.Shared.DTOs.Response;
using Application.VerticalSlice.UrlPart.DTOs;
using System.Security.Claims;


namespace Application.VerticalSlice.UrlPart.Services
{
    public interface IUrlService
    {
        Task<Response> CreateUrlAsync(IEnumerable<Claim> claims,
            CreateUrlDto dto,
            CancellationToken cancellation);
    }
}
