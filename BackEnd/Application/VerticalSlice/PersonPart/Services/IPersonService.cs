using Application.Shared.DTOs.Response;
using Application.VerticalSlice.CompanyPart.DTOs.CreateProfile;
using Application.VerticalSlice.PersonPart.DTOs.Create;
using System.Security.Claims;


namespace Application.VerticalSlice.PersonPart.Services
{
    public interface IPersonService
    {
        Task<Response> CreatePersonProfileAsync
            (
            IEnumerable<Claim> claims,
            CreatePersonRequestDto dto,
            CancellationToken cancellation
            );
    }
}
