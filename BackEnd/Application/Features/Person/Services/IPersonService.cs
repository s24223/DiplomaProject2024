using Application.Features.Person.DTOs.Create;
using Application.Shared.DTOs.Response;
using System.Security.Claims;


namespace Application.Features.Person.Services
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
