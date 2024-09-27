using Domain.Entities.UserPart;

namespace Application.VerticalSlice.UrlPart.Interfaces
{
    public interface IUrlRepository
    {
        Task CreateAsync
            (
            DomainUrl url,
            CancellationToken cancellationToken
            );
    }
}
