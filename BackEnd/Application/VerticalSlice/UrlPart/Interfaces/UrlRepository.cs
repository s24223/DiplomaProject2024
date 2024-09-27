using Application.Database;
using Application.Database.Models;
using Domain.Entities.UserPart;

namespace Application.VerticalSlice.UrlPart.Interfaces
{
    public class UrlRepository : IUrlRepository
    {
        private readonly DiplomaProjectContext _context;

        public UrlRepository
            (
            DiplomaProjectContext context
            )
        {
            _context = context;
        }

        public async Task CreateAsync
            (
            DomainUrl url,
            CancellationToken cancellation
            )
        {
            var databaseUrl = new Url
            {
                UserId = url.Id.UserId.Value,
                UrlTypeId = (int)url.Id.UrlType.Type,
                PublishDate = url.Id.PublishDate,
                Url1 = url.Url.AbsoluteUri,
                Name = url.Name,
                Description = url.Description,
            };
            await _context.Urls.AddAsync(databaseUrl, cancellation);
            await _context.SaveChangesAsync(cancellation);
        }
    }
}
