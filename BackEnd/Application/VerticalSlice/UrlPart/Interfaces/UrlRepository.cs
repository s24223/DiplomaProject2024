using Application.Database;
using Application.Database.Models;
using Application.VerticalSlice.UrlPart.DTOs;
using Domain.Entities.UserPart;
using Domain.Factories;

namespace Application.VerticalSlice.UrlPart.Interfaces
{
    public class UrlRepository : IUrlRepository
    {
        private readonly DiplomaProjectContext _context;
        private readonly IDomainFactory _domainFactory;

        public UrlRepository(
            DiplomaProjectContext context, 
            IDomainFactory domainFactory
            )
        {
            _context = context;
            _domainFactory = domainFactory;
        }

        public async Task CreateUrlAsync(DomainUrl url,
            CancellationToken cancellation)
        {
            await _context.Urls.AddAsync(new Url
            {
                UserId = url.Id.UserId.Value,
                UrlTypeId = (int) url.Id.UrlType.Type,
                PublishDate = url.Id.PublishDate,
                Url1 = url.Url.AbsoluteUri,
                Name = url.Name,
                Description = url.Description,
            });
            await _context.SaveChangesAsync();
        }
    }
}
