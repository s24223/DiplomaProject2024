using Domain.Features.Recruitment.Entities;
using Domain.Features.Recruitment.ValueObjects.Identificators;
using Domain.Features.User.ValueObjects.Identificators;

namespace Application.Features.Internship.RecrutmentPart.Interfaces
{
    public interface IRecruitmentRepository
    {
        //DML
        Task CreateAsync(DomainRecruitment recruitment, CancellationToken cancellation);
        Task SetAnswerAsync
            (
            UserId companyId,
            DomainRecruitment recruitment,
            CancellationToken cancellation
            );

        //DQL
        Task<DomainRecruitment> GetRecruitmentForSetAnswerAsync
            (
            UserId companyId,
            RecrutmentId id,
            CancellationToken cancellation
            );

    }
}
