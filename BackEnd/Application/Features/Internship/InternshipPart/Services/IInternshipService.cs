using Application.Features.Internship.InternshipPart.DTOs;
using Application.Shared.DTOs.Response;
using System.Security.Claims;

namespace Application.Features.Internship.InternshipPart.Services
{
    public interface IInternshipService
    {
        Task<Response> CreateInternshipAsync
            (
            IEnumerable<Claim> claims,
            CreateInternshipDto dto,
            CancellationToken cancellation
            );

        Task<Response> UpdateInternshipAsync(
            Guid id,
            UpdateInternshipDto dto,
            CancellationToken cancellation);
    }
}
