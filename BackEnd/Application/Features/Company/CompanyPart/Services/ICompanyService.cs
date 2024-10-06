using Application.Features.Company.CompanyPart.DTOs.CreateProfile;
using Application.Shared.DTOs.Response;
using System.Security.Claims;

namespace Application.Features.Company.CompanyPart.Services
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
