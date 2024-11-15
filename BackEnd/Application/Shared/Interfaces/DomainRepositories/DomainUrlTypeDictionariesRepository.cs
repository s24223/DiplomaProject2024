using Application.Databases.Relational;
using Domain.Features.Url.Repository;
using Domain.Features.Url.ValueObjects;
using Microsoft.Extensions.Caching.Memory;

namespace Application.Shared.Interfaces.DomainRepositories
{
    public class DomainUrlTypeDictionariesRepository : IDomainUrlTypeDictionariesRepository
    {
        //Values
        private readonly DiplomaProjectContext _context;
        private readonly IMemoryCache _cache;
        private readonly string CacheKeyUrlTypes = "DictionaryUrlTypes";

        //Magic Numbers
        private readonly int _absoluteExpirationHours = 1;
        private readonly int _slidingExpirationFromMinutes = 5;


        //Cosntructor
        public DomainUrlTypeDictionariesRepository
            (
            DiplomaProjectContext context,
            IMemoryCache cache
            )
        {
            _cache = cache;
            _context = context;
        }



        //=================================================================================================================
        //=================================================================================================================
        //=================================================================================================================
        //Public Methods 
        public Dictionary<int, DomainUrlType> GetDomainUrlTypeDictionary()
        {
            if (!_cache.TryGetValue
                 (
                 CacheKeyUrlTypes,
                 out Dictionary<int, DomainUrlType>? dictionary
                 )
                 ||
                 dictionary == null
                 )
            {
                // Jeśli nie ma danych w cache, załaduj je z bazy danych
                dictionary = _context.UrlTypes
                    .Select(e => new DomainUrlType(e.Id, e.Name, e.Description))
                    .ToDictionary(x => x.Id, x => x);

                // Przechowaj dane w cache
                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(_absoluteExpirationHours),
                    SlidingExpiration = TimeSpan.FromMinutes(_slidingExpirationFromMinutes)
                };

                _cache.Set(CacheKeyUrlTypes, dictionary, cacheOptions);
            }
            return dictionary;
        }
        //=================================================================================================================
        //=================================================================================================================
        //=================================================================================================================
        //Private Methods
    }
}
