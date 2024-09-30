using Domain.Entities.RecrutmentPart;

namespace Application.VerticalSlice.InternshipPart.Interfaces
{
    public interface IInternshipRepository
    {
        Task CreateInternshipAsync(
            DomainIntership intership,
            CancellationToken cancellaction);
    }
}
