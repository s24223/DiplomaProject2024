using Application.Shared.DTOs.Features.Users.Urls;
using Domain.Features.Url.Entities;

namespace Application.Features.Users.Queries.QueriesUser.DTOs
{
    public class GetUrlsResp
    {
        //Values
        public IEnumerable<UrlResp> Urls { get; private set; } = [];
        public int Count { get; private set; }
        public int TotalCount { get; private set; }


        //Constructor
        public GetUrlsResp(int totalCount, IEnumerable<DomainUrl> urls)
        {
            TotalCount = totalCount;
            Urls = urls.Select(x => new UrlResp(x));
            Count = urls.Count();
        }
    }
}
