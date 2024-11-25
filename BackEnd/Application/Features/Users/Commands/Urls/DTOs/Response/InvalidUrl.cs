using Application.Shared.DTOs.Features.Users.Urls;
using Domain.Features.Url.Entities;

namespace Application.Features.Users.Commands.Urls.DTOs.Response
{
    public class InvalidUrl
    {
        //Value
        public UrlResp Core { get; init; } = null!;
        public IEnumerable<UrlResp> Duplicates { get; init; } = [];


        //Constructor
        public InvalidUrl(DomainUrl core, IEnumerable<DomainUrl> duplicates)
        {
            Core = new UrlResp(core);
            Duplicates = duplicates.Select(x => new UrlResp(x));
        }
    }
}
