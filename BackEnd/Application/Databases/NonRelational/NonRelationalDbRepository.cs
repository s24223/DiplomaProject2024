using Microsoft.AspNetCore.Http;

namespace Application.Databases.NonRelational
{
    public interface NonRelationalDbRepository
    {
        Task<string> SaveAsync(IFormFile file, CancellationToken cancellation);
        Task<(MemoryStream Stream, string Name)?> GetAsync(string id, CancellationToken cancellation);
    }
}
