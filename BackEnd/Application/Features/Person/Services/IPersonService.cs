using Application.Features.Person.DTOs.Create;
using Application.Features.Person.DTOs.Update;
using Application.Shared.DTOs.Response;
using System.Security.Claims;


namespace Application.Features.Person.Services
{
    public interface IPersonService
    {
        //DML
        Task<Response> CreateAsync
            (
            IEnumerable<Claim> claims,
            CreatePersonRequestDto dto,
            CancellationToken cancellation
            );

        Task<Response> UpdateAsync
            (
            IEnumerable<Claim> claims,
            UpdatePersonRequestDto dto,
            CancellationToken cancellation
            );

        //DQL
    }
}
