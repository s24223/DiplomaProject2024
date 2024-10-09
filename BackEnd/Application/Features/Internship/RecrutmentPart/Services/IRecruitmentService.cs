using Application.Features.Internship.RecrutmentPart.DTOs;
using Application.Features.Internship.RecrutmentPart.DTOs.Create;
using Application.Shared.DTOs.Response;
using System.Security.Claims;

namespace Application.Features.Internship.RecrutmentPart.Services
{
    public interface IRecruitmentService
    {
        Task<Response> CreateAsync
            (
            IEnumerable<Claim> claims,
            CreateRecruitmentRequestDto dto,
            CancellationToken cancellation
            );
        Task<Response> UpdateRecruitmentAsync(
            IEnumerable<Claim> claims,
            UpdateRecrutmentDto dto,
            CancellationToken cancellation);
    }
}
