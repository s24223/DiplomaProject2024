using Application.Shared.DTOs.Response;
using Application.VerticalSlice.CompanyPart.DTOs.CreateProfile;
using System.Security.Claims;

namespace Application.VerticalSlice.CompanyPart.Services
{
    public interface ICompanyService
    {
        Task<Response> CreateCompanyProfileAsync
            (
            IEnumerable<Claim> claims,
            CreateCompanyProfileRequestDto dto,
            CancellationToken cancellation
            );
    }
}
