using Domain.Features.Recruitment.Entities;

namespace Application.Features.Internship.RecrutmentPart.Interfaces
{
    public interface IRecruitmentRepository
    {
        Task CreateAsync(DomainRecruitment recruitment, CancellationToken cancellation);
        Task<DomainRecruitment> GetRecruitmentAsync(Guid id, CancellationToken cancellation);
        Task UpdateRecruitmentAsync(DomainRecruitment recruitment, CancellationToken cancellation);
    }
}
