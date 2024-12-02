using Application.Databases.Relational;
using Application.Databases.Relational.Models;
using Application.Features.Users.Mappers;
using Domain.Features.Url.Entities;
using Domain.Features.Url.Exceptions.Entities;
using Domain.Features.Url.ValueObjects.Identificators;
using Domain.Features.User.ValueObjects.Identificators;
using Domain.Shared.Templates.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Application.Features.Users.Commands.Urls.Interfaces
{
    public class UrlCmdRepo : IUrlCmdRepo
    {
        //Vaues
        private readonly IUserMapper _mapper;
        private readonly DiplomaProjectContext _context;


        //Cosntructor
        public UrlCmdRepo
            (
            IUserMapper mapper,
            DiplomaProjectContext context
            )
        {
            _mapper = mapper;
            _context = context;
        }


        //===================================================================================================
        //===================================================================================================
        //===================================================================================================
        //Methods
        //DML
        public async Task<(IEnumerable<DomainUrl> Database, IEnumerable<DomainUrl> Input)>
            CreateAsync(UserId userId, IEnumerable<DomainUrl> urls, CancellationToken cancellation)
        {
            var timeMistake = 0;
            var dictionary = new Dictionary<string, Url>();

            try
            {
                foreach (var url in urls)
                {
                    timeMistake += 10;
                    var databaseUrl = new Url
                    {
                        UserId = url.Id.UserId.Value,
                        UrlTypeId = url.Id.UrlTypeId,
                        Created = url.Id.Created.AddMilliseconds(timeMistake),
                        Path = url.Path,
                        Name = url.Name,
                        Description = url.Description,
                    };

                    dictionary[url.Path] = databaseUrl;
                }

                await _context.Urls.AddRangeAsync(dictionary.Values, cancellation);
                await _context.SaveChangesAsync(cancellation);

                return (dictionary.Values.Select(x => _mapper.DomainUrl(x)), []);
            }
            catch (System.Exception ex)
            {
                //Url_UNIQUE_Path
                if (IsExceptionConstraintUniquePath(ex))
                {
                    var paths = dictionary.Keys.ToList();
                    return (await GetDatabaseConflictsAsync(userId, paths, cancellation), urls);
                }
                throw;
            }
        }

        public async Task<(IEnumerable<DomainUrl> Database, IEnumerable<DomainUrl> Input)>
            UpdateAsync(UserId userId, Dictionary<UrlId, DomainUrl> urls, CancellationToken cancellation)
        {
            var dictionary = await GetDatabaseUrlsDictionaryAsync(urls.Keys, cancellation);

            try
            {
                foreach (var key in urls.Keys)
                {
                    var input = urls[key];
                    var database = dictionary[key];

                    database.Name = input.Name;
                    database.Description = input.Description;
                    database.Path = input.Path;
                }

                await _context.SaveChangesAsync(cancellation);
                return (dictionary.Values.Select(x => _mapper.DomainUrl(x)), []);
            }
            catch (System.Exception ex)
            {
                if (IsExceptionConstraintUniquePath(ex))
                {
                    var paths = dictionary.Values.Select(x => x.Path);
                    return (await GetDatabaseConflictsAsync(userId, paths, cancellation), urls.Values);
                }
                throw;
            }
        }

        public async Task DeleteAsync(IEnumerable<UrlId> ids, CancellationToken cancellation)
        {
            var dictionary = await GetDatabaseUrlsDictionaryAsync(ids, cancellation);
            foreach (var databaseUrl in dictionary)
            {
                _context.Urls.Remove(databaseUrl.Value);
            }

            await _context.SaveChangesAsync(cancellation);
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
                x => _mapper.DomainUrl(x.Value)
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
            var userId = ids.FirstOrDefault()?.UserId.Value;

            var list = await _context.Urls
                .Where(x => x.UserId == userId)
                .ToListAsync(cancellation);

            var urls = list.Where(x => ids.Any(y =>
                    y.Created == x.Created &&
                    y.UrlTypeId == x.UrlTypeId
                )).ToDictionary
                (
                x => new UrlId(new UserId(x.UserId), x.UrlTypeId, x.Created),
                x => x
                );

            var missingIds = ids.Where(id => !urls.ContainsKey(id));
            if (missingIds.Any())
            {
                var builder = new StringBuilder();
                builder.AppendLine(Messages.Url_Cmd_Ids_NotFound);
                builder.AppendLine($"UserId,\t UrlTypeId,\t Created");

                foreach (var id in missingIds)
                {
                    builder.AppendLine($"{id.UserId.Value},\t {id.UrlTypeId},\t {id.Created}");
                }

                throw new UrlException
                    (
                    builder.ToString(),
                    DomainExceptionTypeEnum.NotFound
                    );
            }
            return urls;
        }

        private bool IsExceptionConstraintUniquePath(System.Exception ex)
        {
            if (
                ex is DbUpdateException &&
                ex.InnerException is SqlException sqlEx &&
                sqlEx.Message.Contains("Url_UNIQUE_Path")
                )
            {
                return true;
            }
            return false;
        }

        private async Task<IEnumerable<DomainUrl>> GetDatabaseConflictsAsync
            (UserId userId, IEnumerable<string> paths, CancellationToken cancellation)
        {
            var databaseItems = await _context.Urls
                        .Where(x =>
                            x.UserId == userId.Value &&
                            paths.Contains(x.Path))
                        .ToListAsync(cancellation);

            return databaseItems.Select(x => _mapper.DomainUrl(x));
        }
    }
}
