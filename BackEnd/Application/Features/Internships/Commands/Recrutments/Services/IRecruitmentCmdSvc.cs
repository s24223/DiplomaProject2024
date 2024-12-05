using Application.Features.Internships.Commands.Recrutments.DTOs;
using Application.Shared.DTOs.Features.Internships;
using Application.Shared.DTOs.Response;
using System.Security.Claims;

namespace Application.Features.Internships.Commands.Recrutments.Services
{
    public interface IRecruitmentCmdSvc
    {
        //DML
        Task<ResponseItem<RecruitmentResp>> CreateByPersonAsync
            (
            IEnumerable<Claim> claims,
            Guid branchOfferId,
            CreateRecruitmentReq dto,
            CancellationToken cancellation
            );

        Task<ResponseItem<RecruitmentResp>> SetAnswerByCompanyAsync
            (
            IEnumerable<Claim> claims,
            Guid recrutmentId,
            SetAnswerReq dto,
            CancellationToken cancellation
            );
    }
}
