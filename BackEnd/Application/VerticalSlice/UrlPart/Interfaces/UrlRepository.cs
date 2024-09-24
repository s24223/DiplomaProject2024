using Application.Database;
using Application.Database.Models;
using Domain.Entities.UserPart;
using Domain.Factories;

namespace Application.VerticalSlice.UrlPart.Interfaces
{
    public class UrlRepository : IUrlRepository
    {
        private readonly DiplomaProjectContext _context;

        public UrlRepository(
            DiplomaProjectContext context
            )
        {
            _context = context;
        }

        public async Task CreateUrlAsync(DomainUrl url,
            CancellationToken cancellation)
        {
            await _context.Urls.AddAsync(new Url
            {
                UserId = url.Id.UserId.Value,
                UrlTypeId = (int)url.Id.UrlType.Type,
                PublishDate = url.Id.PublishDate,
                Url1 = url.Url.AbsoluteUri,
                Name = url.Name,
                Description = url.Description,
            });
            await _context.SaveChangesAsync();
        }
    }
}
