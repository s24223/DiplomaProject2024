using Application.Features.Companies.DTOs.CommandsCompanyBranch.CommandsBranch.Create;
using Application.Features.Companies.DTOs.CommandsCompanyBranch.CommandsBranch.Update;
using Application.Features.Companies.DTOs.CommandsCompanyBranch.CommandsCompany.Create;
using Application.Features.Companies.DTOs.CommandsCompanyBranch.CommandsCompany.Update;
using Application.Shared.DTOs.Features.Companies.Responses;
using Application.Shared.DTOs.Response;
using System.Security.Claims;

namespace Application.Features.Companies.Services.CommandsCompanyBranch
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

        Task<ResponseItem<CompanyResponseDto>> UpdateCompanyAsync
            (
            IEnumerable<Claim> claims,
            UpdateCompanyRequestDto dto,
            CancellationToken cancellation
            );

        //Branch
        Task<ResponseItems<BranchResponseDto>> CreateBranchesAsync
            (
            IEnumerable<Claim> claims,
            IEnumerable<CreateBranchRequestDto> dtos,
            CancellationToken cancellation
            );

        Task<ResponseItems<BranchResponseDto>> UpdateBranchesAsync
            (
            IEnumerable<Claim> claims,
            IEnumerable<UpdateBranchRequestDto> dtos,
            CancellationToken cancellation
            );
    }
}
