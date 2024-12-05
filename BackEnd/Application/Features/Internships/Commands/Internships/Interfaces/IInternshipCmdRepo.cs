using Domain.Features.Intership.Entities;
using Domain.Features.Recruitment.ValueObjects.Identificators;
using Domain.Features.User.ValueObjects.Identificators;

namespace Application.Features.Internships.Commands.Internships.Interfaces
{
    public interface IInternshipCmdRepo
    {
        //DML
        Task<DomainIntership> CreateAsync
            (
            UserId companyId,
            DomainIntership domain,
            CancellationToken cancellation
            );

        Task<DomainIntership> UpdateAsync
            (
            UserId companyId,
            DomainIntership domain,
            CancellationToken cancellation
            );

        //DQL
        Task<DomainIntership> GetInternshipAsync
            (
            UserId companyId,
            RecrutmentId intershipId,
            CancellationToken cancellation
            );
    }
}
