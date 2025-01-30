using Domain.Features.Recruitment.Entities;
using Domain.Features.Recruitment.ValueObjects.Identificators;
using Domain.Features.User.ValueObjects.Identificators;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Internships.Commands.Recrutments.Interfaces
{
    public interface IRecruitmentCmdRepo
    {
        //DML
        Task<DomainRecruitment> CreateAsync
            (
            DomainRecruitment domain,
            IFormFile? file,
            CancellationToken cancellation
            );

        Task<DomainRecruitment> UpdateAsync
            (
            UserId companyId,
            DomainRecruitment domain,
            CancellationToken cancellation
            );

        //DQL
        Task<DomainRecruitment> GetRecruitmentAsync
            (
            UserId companyId,
            RecrutmentId recrutmentid,
            CancellationToken cancellation
            );

    }
}
