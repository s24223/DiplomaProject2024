using Application.Shared.DTOs.Features.Users.Urls;
using Domain.Features.Url.Entities;

namespace Application.Features.Users.Commands.Urls.DTOs.Response
{
    public class DmlUrlsResp
    {
        //Values
        public IEnumerable<UrlResp> Correct { get; init; } = [];
        public int CorrectCount { get; private init; } = 0;
        public IEnumerable<InvalidUrl> InvalidUrls { get; private init; } = [];


        //Constructor
        public DmlUrlsResp
            (
            IEnumerable<DomainUrl> correct,
            Dictionary<DomainUrl, List<DomainUrl>> invalid
            )
        {
            Correct = correct.Select(x => new UrlResp(x));
            CorrectCount = correct.Count();
            InvalidUrls = invalid.Select(pair => new InvalidUrl(pair.Key, pair.Value));
        }

        public DmlUrlsResp
            (
            IEnumerable<DomainUrl> correct,
            Dictionary<DomainUrl, DomainUrl> invalid
            )
        {
            Correct = correct.Select(x => new UrlResp(x));
            CorrectCount = correct.Count();
            InvalidUrls = invalid.Select(pair => new InvalidUrl(pair.Key, [pair.Value]));
        }

        public DmlUrlsResp
            (
            IEnumerable<DomainUrl> correct
            )
        {
            Correct = correct.Select(x => new UrlResp(x));
            CorrectCount = correct.Count();
        }
    }
}
