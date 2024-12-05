using Application.Features.Internships.Commands.Internships.DTOs;
using Application.Shared.DTOs.Features.Internships;
using Application.Shared.DTOs.Response;
using System.Security.Claims;

namespace Application.Features.Internships.Commands.Internships.Services
{
    public interface IInternshipCmdSvc
    {
        Task<ResponseItem<InternshipResp>> CreateAsync
            (
            IEnumerable<Claim> claims,
            Guid recrutmentId,
            CreateInternshipReq dto,
            CancellationToken cancellation
            );

        Task<ResponseItem<InternshipResp>> UpdateAsync
            (
            IEnumerable<Claim> claims,
            Guid internshipId,
            UpdateInternshipReq dto,
            CancellationToken cancellation
            );
    }
}
