using Application.VerticalSlice.UrlPart.DTOs;
using Domain.Entities.UserPart;

namespace Application.VerticalSlice.UrlPart.Interfaces
{
    public interface IUrlRepository
    {
        Task CreateUrlAsync(DomainUrl url, 
            CancellationToken cancellationToken);
    }
}
