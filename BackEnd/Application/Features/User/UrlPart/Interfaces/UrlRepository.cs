using Application.Database;
using Application.Database.Models;
using Application.Shared.Interfaces.Exceptions;
using Domain.Features.Url.Entities;
using Domain.Features.Url.Exceptions;
using Domain.Features.User.ValueObjects.Identificators;
using Domain.Shared.Factories;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.User.UrlPart.Interfaces
{
    public class UrlRepository : IUrlRepository
    {
        //vaues
        private readonly IDomainFactory _domainFactory;
        private readonly IExceptionsRepository _exceptionsRepository;
        private readonly DiplomaProjectContext _context;


        //Cosntructor
        public UrlRepository
            (
            IDomainFactory domainFactory,
            IExceptionsRepository exceptionsRepository,
            DiplomaProjectContext context
            )
        {
            _domainFactory = domainFactory;
            _exceptionsRepository = exceptionsRepository;
            _context = context;
        }


        //Methods
        //===================================================================================================
        //DML
        public async Task CreateAsync
            (
            DomainUrl url,
            CancellationToken cancellation
            )
        {
            try
            {
                var databaseUrl = new Url
                {
                    UserId = url.Id.UserId.Value,
                    UrlTypeId = (int)url.Id.UrlType.Type,
                    Created = url.Id.Created,
                    Path = url.Path.ToString(),
                    Name = url.Name,
                    Description = url.Description,
                };
                await _context.Urls.AddAsync(databaseUrl, cancellation);
                await _context.SaveChangesAsync(cancellation);
            }
            catch (System.Exception ex)
            {
                throw _exceptionsRepository.ConvertEFDbException
                    (
                    ex
                    );
            }
        }

        public async Task UpdateAsync
            (
            DomainUrl url,
            CancellationToken cancellation
            )
        {
            try
            {
                var databaseUrl = await GetDatabaseUrlAsync
                (
                url.Id.UserId,
                url.Id.UrlType,
                url.Id.Created,
                cancellation
                );

                databaseUrl.Name = url.Name;
                databaseUrl.Description = url.Description;
                databaseUrl.Path = url.Path.ToString();

                await _context.SaveChangesAsync(cancellation);
            }
            catch (System.Exception ex)
            {
                throw _exceptionsRepository.ConvertEFDbException
                    (
                    ex
                    );
            }
        }

        public async Task DeleteAsync
            (
            UserId userId,
            Domain.Features.Url.ValueObjects.UrlTypePart.UrlType urlType,
            DateTime created,
            CancellationToken cancellation
            )
        {
            try
            {
                var databaseUrl = await GetDatabaseUrlAsync
                (
                userId,
                urlType,
                created,
                cancellation
                );
                _context.Urls.Remove(databaseUrl);

                await _context.SaveChangesAsync(cancellation);
            }
            catch (System.Exception ex)
            {
                throw _exceptionsRepository.ConvertEFDbException
                    (
                    ex
                    );
            }
        }

        //====================================================================================================
        //DQL
        public async Task<DomainUrl> GetUrlAsync
            (
            UserId userId,
            Domain.Features.Url.ValueObjects.UrlTypePart.UrlType urlType,
            DateTime created,
            CancellationToken cancellation
            )
        {
            var databaseUrl = await GetDatabaseUrlAsync
                (
                userId,
                urlType,
                created,
                cancellation
                );
            return _domainFactory.CreateDomainUrl
                (
                databaseUrl.UserId,
                databaseUrl.UrlTypeId,
                databaseUrl.Created,
                databaseUrl.Path,
                databaseUrl.Name,
                databaseUrl.Description
                );
        }
        public async Task<IEnumerable<DomainUrl>> GetUrlsAsync
            (
            UserId userId,
            CancellationToken cancellation
            )
        {
            return await _context.Urls
                .Where(x => x.UserId == userId.Value)
                .Select(databaseUrl => _domainFactory.CreateDomainUrl
                (
                databaseUrl.UserId,
                databaseUrl.UrlTypeId,
                databaseUrl.Created,
                databaseUrl.Path,
                databaseUrl.Name,
                databaseUrl.Description
                )).ToListAsync(cancellation);
        }

        //====================================================================================================
        //====================================================================================================
        //====================================================================================================
        //Private Methods
        private async Task<Url> GetDatabaseUrlAsync
            (
            UserId userId,
            Domain.Features.Url.ValueObjects.UrlTypePart.UrlType urlType,
            DateTime created,
            CancellationToken cancellation
            )
        {
            var url = await _context.Urls
                .Where(x =>
                x.UserId == userId.Value &&
                x.UrlTypeId == (int)urlType.Type &&
                x.Created == created
                ).FirstOrDefaultAsync(cancellation);
            if (url == null)
            {
                throw new UrlException(Messages.NotExistUrl);
            }
            return url;
        }

    }
}
