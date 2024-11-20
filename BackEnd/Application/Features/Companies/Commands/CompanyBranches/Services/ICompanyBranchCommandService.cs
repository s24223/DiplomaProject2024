using Application.Features.Companies.Commands.CompanyBranches.DTOs.CommandsBranch.Create;
using Application.Features.Companies.Commands.CompanyBranches.DTOs.CommandsBranch.Update;
using Application.Features.Companies.Commands.CompanyBranches.DTOs.CommandsCompany.Create;
using Application.Features.Companies.Commands.CompanyBranches.DTOs.CommandsCompany.Update;
using Application.Shared.DTOs.Features.Companies.Responses;
using Application.Shared.DTOs.Response;
using System.Security.Claims;

namespace Application.Features.Companies.Commands.CompanyBranches.Services
{
    public interface ICompanyBranchCommandService
    {
        //Company
        Task<ResponseItem<CreateCompanyResponseDto>> CreateAsync
            (
            IEnumerable<Claim> claims,
            CreateCompanyRequestDto dto,
            CancellationToken cancellation
            );

        Task<ResponseItem<CompanyResp>> UpdateCompanyAsync
            (
            IEnumerable<Claim> claims,
            UpdateCompanyRequestDto dto,
            CancellationToken cancellation
            );

        //Branch
        Task<ResponseItems<BranchResp>> CreateBranchesAsync
            (
            IEnumerable<Claim> claims,
            IEnumerable<CreateBranchRequestDto> dtos,
            CancellationToken cancellation
            );

        Task<ResponseItems<BranchResp>> UpdateBranchesAsync
            (
            IEnumerable<Claim> claims,
            IEnumerable<UpdateBranchRequestDto> dtos,
            CancellationToken cancellation
            );
    }
}
