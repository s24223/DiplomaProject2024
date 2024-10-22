using Domain.Features.Url.Entities;
using Domain.Features.Url.ValueObjects.Identificators;

namespace Application.Features.User.Interfaces.CommandsUrl
{
    public interface IUrlCommandRepository
    {
        //===================================================================================================
        //DML
        Task CreateAsync
            (
            IEnumerable<DomainUrl> urls,
            CancellationToken cancellation
            );

        Task UpdateAsync
            (
            Dictionary<UrlId, DomainUrl> urls,
            CancellationToken cancellation
            );

        Task DeleteAsync
            (
            IEnumerable<UrlId> ids,
            CancellationToken cancellation
            );

        //===================================================================================================
        //DQL
        Task<Dictionary<UrlId, DomainUrl>> GetUrlsDictionaryAsync
            (
            IEnumerable<UrlId> ids,
            CancellationToken cancellation
            );
    }
}
