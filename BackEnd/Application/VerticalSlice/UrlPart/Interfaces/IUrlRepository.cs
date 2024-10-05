﻿using Domain.VerticalSlice.UrlPart.Entities;
using Domain.VerticalSlice.UrlPart.ValueObjects.UrlTypePart;
using Domain.VerticalSlice.UserPart.ValueObjects.Identificators;

namespace Application.VerticalSlice.UrlPart.Interfaces
{
    public interface IUrlRepository
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

        Task<IEnumerable<DomainUrl>> GetUrlsAsync
            (
            UserId userId,
            CancellationToken cancellation
            );
    }
}
