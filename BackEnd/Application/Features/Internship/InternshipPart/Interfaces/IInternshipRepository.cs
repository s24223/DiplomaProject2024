using Domain.Features.Intership.Entities;

namespace Application.Features.Internship.InternshipPart.Interfaces
{
    public interface IInternshipRepository
    {
        Task CreateInternshipAsync(
            DomainIntership intership,
            CancellationToken cancellaction);
    }
}
