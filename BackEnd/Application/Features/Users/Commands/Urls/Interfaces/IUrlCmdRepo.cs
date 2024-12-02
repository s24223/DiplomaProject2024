using Domain.Features.Url.Entities;
using Domain.Features.Url.ValueObjects.Identificators;
using Domain.Features.User.ValueObjects.Identificators;

namespace Application.Features.Users.Commands.Urls.Interfaces
{
    public interface IUrlCmdRepo
    {
        //===================================================================================================
        //DML
        Task<(IEnumerable<DomainUrl> Database, IEnumerable<DomainUrl> Input)>
           CreateAsync(UserId userId, IEnumerable<DomainUrl> urls, CancellationToken cancellation);

        Task<(IEnumerable<DomainUrl> Database, IEnumerable<DomainUrl> Input)>
             UpdateAsync(UserId userId, Dictionary<UrlId, DomainUrl> urls, CancellationToken cancellation);

        Task DeleteAsync(IEnumerable<UrlId> ids, CancellationToken cancellation);

        //===================================================================================================
        //DQL
        Task<Dictionary<UrlId, DomainUrl>> GetUrlsDictionaryAsync
            (
            IEnumerable<UrlId> ids,
            CancellationToken cancellation
            );
    }
}
