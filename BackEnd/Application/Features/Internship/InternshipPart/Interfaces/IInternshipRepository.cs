using Application.Features.Internship.InternshipPart.DTOs;
using Domain.Features.Intership.Entities;

namespace Application.Features.Internship.InternshipPart.Interfaces
{
    public interface IInternshipRepository
    {
        Task CreateInternshipAsync(
            DomainIntership intership,
            CancellationToken cancellaction);

        Task UpdateInternshipAsync(
            DomainIntership intership,
            CancellationToken cancellaction);

        Task<DomainIntership> GetInternshipAsync(Guid id,
            CancellationToken cancellation);
    }
}
