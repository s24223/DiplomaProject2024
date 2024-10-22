using Application.Databases.Relational;
using Domain.Features.Notification.Repositories;
using Domain.Features.Notification.ValueObjects;
using Microsoft.Extensions.Caching.Memory;

namespace Application.Shared.Repositories
{
    public class DomainNotificationDictionariesRepository : IDomainNotificationDictionariesRepository
    {
        //Values
        private readonly DiplomaProjectContext _context;
        private readonly IMemoryCache _cache;
        private readonly string CacheKeyNotificationSenders = "DictionaryNotificationSenders";
        private readonly string CacheKeyNotificationStatuses = "DictionaryNotificationStatuses";

        //Magic Numbers
        private readonly int _absoluteExpirationHours = 1;
        private readonly int _slidingExpirationFromMinutes = 5;


        //Cosntructor
        public DomainNotificationDictionariesRepository
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
        public Dictionary<int, DomainNotificationSender> GetNotificationSenders()
        {
            if (!_cache.TryGetValue
                (
                CacheKeyNotificationSenders,
                out Dictionary<int, DomainNotificationSender>? dictionary
                )
                ||
                dictionary == null
                )
            {
                // Jeśli nie ma danych w cache, załaduj je z bazy danych
                dictionary = _context.NotificationSenders
                    .Select(e => new DomainNotificationSender(e.Id, e.Name, e.Description))
                    .ToDictionary(x => x.Id, x => x);

                // Przechowaj dane w cache
                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(_absoluteExpirationHours),
                    SlidingExpiration = TimeSpan.FromMinutes(_slidingExpirationFromMinutes)
                };

                _cache.Set(CacheKeyNotificationSenders, dictionary, cacheOptions);
            }
            return dictionary;
        }

        public Dictionary<int, DomainNotificationStatus> GetNotificationStatuses()
        {
            if (!_cache.TryGetValue
               (
               CacheKeyNotificationStatuses,
               out Dictionary<int, DomainNotificationStatus>? dictionary
               )
               ||
               dictionary == null
               )
            {
                // Jeśli nie ma danych w cache, załaduj je z bazy danych
                dictionary = _context.NotificationStatuses
                    .Select(e => new DomainNotificationStatus(e.Id, e.Name))
                    .ToDictionary(x => x.Id, x => x);

                // Przechowaj dane w cache
                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(_absoluteExpirationHours),
                    SlidingExpiration = TimeSpan.FromMinutes(_slidingExpirationFromMinutes)
                };

                _cache.Set(CacheKeyNotificationStatuses, dictionary, cacheOptions);
            }
            return dictionary;
        }

        //=================================================================================================================
        //=================================================================================================================
        //=================================================================================================================
        //Private Methods 
    }
}
