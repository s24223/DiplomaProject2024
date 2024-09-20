using Application.VerticalSlice.CompanyPart.DTOs.CreateProfile;
using System.Security.Claims;

namespace Application.VerticalSlice.CompanyPart.Services
{
    public interface ICompanyService
    {
        Task CreateCompanyProfileAsync
            (
            IEnumerable<Claim> claims,
            CreateCompanyProfileRequestDto dto,
            CancellationToken cancellation
            );
    }
}
