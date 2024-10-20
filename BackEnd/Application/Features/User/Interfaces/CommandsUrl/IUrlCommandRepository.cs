using Domain.Features.Url.Entities;
using Domain.Features.Url.ValueObjects.UrlTypePart;
using Domain.Features.User.ValueObjects.Identificators;

namespace Application.Features.User.Interfaces.CommandsUrl
{
    public interface IUrlCommandRepository
    {
        //===================================================================================================
        //DML
        Task CreateAsync
            (
            DomainUrl url,
            CancellationToken cancellationToken
            );
        Task UpdateAsync
            (
            DomainUrl url,
            CancellationToken cancellation
            );
        Task DeleteAsync
            (
            UserId userId,
            UrlType urlType,
            DateTime created,
            CancellationToken cancellation
            );

        //===================================================================================================
        //DQL
        Task<DomainUrl> GetUrlAsync
            (
            UserId userId,
            UrlType urlType,
            DateTime created,
            CancellationToken cancellation
            );
    }
}
