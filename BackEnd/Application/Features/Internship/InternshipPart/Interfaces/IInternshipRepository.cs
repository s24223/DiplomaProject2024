using Domain.Features.Intership.Entities;
using Domain.Features.Intership.ValueObjects.Identificators;
using Domain.Features.User.ValueObjects.Identificators;

namespace Application.Features.Internship.InternshipPart.Interfaces
{
    public interface IInternshipRepository
    {
        //DML
        Task<Guid> CreateAsync
            (
            UserId companyId,
            DomainIntership intership,
            CancellationToken cancellation
            );

        Task UpdateAsync
            (
            UserId companyId,
            DomainIntership intership,
            CancellationToken cancellation
            );

        //DQL
        Task<DomainIntership> GetInternshipAsync
            (
            UserId companyId,
            IntershipId intershipId,
            CancellationToken cancellation
            );
    }
}
