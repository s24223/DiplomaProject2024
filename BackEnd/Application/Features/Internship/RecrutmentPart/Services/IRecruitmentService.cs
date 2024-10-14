using Application.Features.Internship.RecrutmentPart.DTOs.Create;
using Application.Features.Internship.RecrutmentPart.DTOs.SetAnswerByCompany;
using Application.Shared.DTOs.Response;
using System.Security.Claims;

namespace Application.Features.Internship.RecrutmentPart.Services
{
    public interface IRecruitmentService
    {
        //DML
        Task<Response> CreateByPersonAsync
            (
            IEnumerable<Claim> claims,
            Guid branchId,
            Guid offerId,
            DateTime created,
            CreateRecruitmentRequestDto dto,
            CancellationToken cancellation
            );

        Task<Response> SetAnswerByCompanyAsync
            (
            IEnumerable<Claim> claims,
            Guid branchId,
            Guid offerId,
            DateTime created,
            Guid personId,
            SetAnswerByCompanyRecrutmentDto dto,
            CancellationToken cancellation
            );

        //DQL
    }
}
