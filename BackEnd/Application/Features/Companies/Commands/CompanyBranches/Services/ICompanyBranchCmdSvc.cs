using Application.Features.Companies.Commands.CompanyBranches.DTOs.CommandsBranch.Create;
using Application.Features.Companies.Commands.CompanyBranches.DTOs.CommandsBranch.Update;
using Application.Features.Companies.Commands.CompanyBranches.DTOs.CommandsCompany.Create;
using Application.Features.Companies.Commands.CompanyBranches.DTOs.CommandsCompany.Update;
using Application.Shared.DTOs.Features.Companies.Responses;
using Application.Shared.DTOs.Response;
using System.Security.Claims;

namespace Application.Features.Companies.Commands.CompanyBranches.Services
{
    public interface ICompanyBranchCmdSvc
    {
        //Company
        Task<ResponseItem<CreateCompanyResp>> CreateAsync
            (
            IEnumerable<Claim> claims,
            CreateCompanyReq dto,
            CancellationToken cancellation
            );

        Task<ResponseItem<CompanyResp>> UpdateCompanyAsync
            (
            IEnumerable<Claim> claims,
            UpdateCompanyReq dto,
            CancellationToken cancellation
            );

        //Branch
        Task<ResponseItems<CreateBranchesResp>> CreateBranchesAsync
            (
            IEnumerable<Claim> claims,
            IEnumerable<CreateBranchReq> dtos,
            CancellationToken cancellation
            );

        Task<ResponseItems<UpdateBranchesResp>> UpdateBranchesAsync
            (
            IEnumerable<Claim> claims,
            IEnumerable<UpdateBranchRequestDto> dtos,
            CancellationToken cancellation
            );
    }
}
