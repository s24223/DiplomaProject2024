using Application.Shared.DTOs.Response;
using Application.VerticalSlice.RecrutmentPart.DTOs.Create;
using System.Security.Claims;

namespace Application.VerticalSlice.RecrutmentPart.Services
{
    public interface IRecruitmentService
    {
        Task<Response> CreateAsync
            (
            IEnumerable<Claim> claims,
            CreateRecruitmentRequestDto dto,
            CancellationToken cancellation
            );
    }
}
