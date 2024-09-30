using Application.Shared.DTOs.Response;
using Application.VerticalSlice.InternshipPart.DTOs;
using System.Security.Claims;

namespace Application.VerticalSlice.InternshipPart.Services
{
    public interface IInternshipService
    {
        Task<Response> CreateInternshipAsync
            (
            IEnumerable<Claim> claims,
            CreateInternshipDto dto,
            CancellationToken cancellation
            );
    }
}
