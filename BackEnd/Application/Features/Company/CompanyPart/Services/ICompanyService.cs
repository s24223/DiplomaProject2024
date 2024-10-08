using Application.Features.Company.CompanyPart.DTOs.Create;
using Application.Features.Company.CompanyPart.DTOs.Update;
using Application.Shared.DTOs.Response;
using System.Security.Claims;

namespace Application.Features.Company.CompanyPart.Services
{
    public interface ICompanyService
    {
        //DML
        Task<Response> CreateCompanyAsync
            (
            IEnumerable<Claim> claims,
            CreateCompanyRequestDto dto,
            CancellationToken cancellation
            );

        Task<Response> UpdateCompanyAsync
            (
            IEnumerable<Claim> claims,
            UpdateCompanyRequestDto dto,
            CancellationToken cancellation
            );

        //DQL
    }
}
