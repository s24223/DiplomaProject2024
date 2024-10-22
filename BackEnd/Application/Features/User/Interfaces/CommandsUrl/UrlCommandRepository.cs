using Application.Databases.Relational;
using Application.Databases.Relational.Models;
using Application.Shared.Interfaces.EntityToDomainMappers;
using Application.Shared.Interfaces.Exceptions;
using Domain.Features.Url.Entities;
using Domain.Features.Url.Exceptions.Entities;
using Domain.Features.Url.ValueObjects.Identificators;
using Domain.Features.User.ValueObjects.Identificators;
using Domain.Shared.Templates.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Application.Features.User.Interfaces.CommandsUrl
{
    public class UrlCommandRepository : IUrlCommandRepository
    {
        //Vaues
        private readonly IEntityToDomainMapper _mapper;
        private readonly IExceptionsRepository _exceptionsRepository;
        private readonly DiplomaProjectContext _context;


        //Cosntructor
        public UrlCommandRepository
            (
            IEntityToDomainMapper mapper,
            IExceptionsRepository exceptionsRepository,
            DiplomaProjectContext context
            )
        {
            _mapper = mapper;
            _exceptionsRepository = exceptionsRepository;
            _context = context;
        }


        //===================================================================================================
        //===================================================================================================
        //===================================================================================================
        //Methods
        //DML
        public async Task CreateAsync
            (
            IEnumerable<DomainUrl> urls,
            CancellationToken cancellation
            )
        {
            try
            {
                var timeMistake = 0;

                foreach (var url in urls)
                {
                    var databaseUrl = new Url
                    {
                        UserId = url.Id.UserId.Value,
                        UrlTypeId = url.Id.UrlTypeId,
                        Created = url.Id.Created.AddMilliseconds(timeMistake++),
                        Path = url.Path,
                        Name = url.Name,
                        Description = url.Description,
                    };
                    await _context.Urls.AddAsync(databaseUrl, cancellation);
                }
                await _context.SaveChangesAsync(cancellation);
            }
            catch (System.Exception ex)
            {
                throw _exceptionsRepository.ConvertEFDbException(ex);
            }
        }

        public async Task UpdateAsync
            (
            Dictionary<UrlId, DomainUrl> urls,
            CancellationToken cancellation
            )
        {
            try
            {
                var databaseUrlsDictionary = await GetDatabaseUrlsDictionaryAsync(urls.Keys, cancellation);

                foreach (var databaseUrl in databaseUrlsDictionary)
                {
                    if (urls.TryGetValue(databaseUrl.Key, out var domainUrl))
                    {
                        databaseUrl.Value.Name = domainUrl.Name;
                        databaseUrl.Value.Description = domainUrl.Description;
                        databaseUrl.Value.Path = domainUrl.Path;
                    }
                }
                await _context.SaveChangesAsync(cancellation);
            }
            catch (System.Exception ex)
            {
                throw _exceptionsRepository.ConvertEFDbException(ex);
            }
        }

        public async Task DeleteAsync
            (
            IEnumerable<UrlId> ids,
            CancellationToken cancellation
            )
        {
            try
            {
                var databaseUrlsDictionary = await GetDatabaseUrlsDictionaryAsync(ids, cancellation);

                foreach (var databaseUrl in databaseUrlsDictionary)
                {
                    _context.Urls.Remove(databaseUrl.Value);
                }
                await _context.SaveChangesAsync(cancellation);
            }
            catch (System.Exception ex)
            {
                throw _exceptionsRepository.ConvertEFDbException(ex);
            }
        }

        //====================================================================================================
        //DQL
        public async Task<Dictionary<UrlId, DomainUrl>> GetUrlsDictionaryAsync
            (
            IEnumerable<UrlId> ids,
            CancellationToken cancellation
            )
        {
            var databaseUrl = await GetDatabaseUrlsDictionaryAsync(ids, cancellation);

            return databaseUrl.ToDictionary
                (
                x => x.Key,
                x => _mapper.ToDomainUrl(x.Value)
                );
        }

        //====================================================================================================
        //====================================================================================================
        //====================================================================================================
        //Private Methods
        private async Task<Dictionary<UrlId, Url>> GetDatabaseUrlsDictionaryAsync
            (
            IEnumerable<UrlId> ids,
            CancellationToken cancellation
            )
        {
            var idSet = new HashSet<UrlId>(ids);
            var urls = await _context.Urls
                .Where(x => idSet.Any(y =>
                    y.UserId.Value == x.UserId &&
                    y.UrlTypeId == x.UrlTypeId &&
                    y.Created == x.Created
                )).ToDictionaryAsync(x =>
                    new UrlId(new UserId(x.UserId), x.UrlTypeId, x.Created),
                    x => x
                    );

            var missingIds = ids.Where(id => !urls.ContainsKey(id));


            if (missingIds.Any())
            {
                var builder = new StringBuilder();
                builder.AppendLine(Messages.Url_Ids_NotFound);
                builder.AppendLine($"UserId,\t UrlTypeId,\t Created");

                foreach (var id in missingIds)
                {
                    builder.AppendLine($"{id.UserId},\t {id.UrlTypeId},\t {id.Created}");
                }

                throw new UrlException
                    (
                    builder.ToString(),
                    DomainExceptionTypeEnum.NotFound
                    );
            }
            return urls;
        }
    }
}
