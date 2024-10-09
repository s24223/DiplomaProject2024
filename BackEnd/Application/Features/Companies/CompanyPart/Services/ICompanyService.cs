using Application.Features.Companies.CompanyPart.DTOs.Create;
using Application.Features.Companies.CompanyPart.DTOs.Update;
using Application.Shared.DTOs.Response;
using System.Security.Claims;

namespace Application.Features.Companies.CompanyPart.Services
{
    public interface ICompanyService
    {
        //DML
        Task<Response> CreateAsync
            (
            IEnumerable<Claim> claims,
            CreateCompanyRequestDto dto,
            CancellationToken cancellation
            );

        Task<Response> UpdateAsync
            (
            IEnumerable<Claim> claims,
            UpdateCompanyRequestDto dto,
            CancellationToken cancellation
            );

        //DQL
    }
}
