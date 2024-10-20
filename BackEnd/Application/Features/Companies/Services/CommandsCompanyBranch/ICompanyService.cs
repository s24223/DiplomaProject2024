using Application.Features.Companies.DTOs.CommandsCompanyBranch.C.Create;
using Application.Features.Companies.DTOs.CommandsCompanyBranch.C.Update;
using Application.Shared.DTOs.Response;
using System.Security.Claims;

namespace Application.Features.Companies.Services.CommandsCompanyBranch
{
    public interface ICompanyService
    {
        //DML
        Task<ResponseItem<CreateCompanyResponseDto>> CreateAsync
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
