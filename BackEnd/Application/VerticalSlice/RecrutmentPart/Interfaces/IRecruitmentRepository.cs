using Domain.VerticalSlice.RecruitmentPart.Entities;

namespace Application.VerticalSlice.RecrutmentPart.Interfaces
{
    public interface IRecruitmentRepository
    {
        Task CreateAsync(DomainRecruitment recruitment, CancellationToken cancellation);
    }
}
