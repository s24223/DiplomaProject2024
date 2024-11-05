using Application.Databases.Relational;
using Domain.Features.Characteristic.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Application.Features.Characteristics.Interfaces.Queries
{
    public class CharacteristicRepository : ICharacteristicRepository
    {
        //Values
        private readonly DiplomaProjectContext _context;
        private readonly IMemoryCache _cache;
        private readonly string _cacheKeyCharacteristics = "DictionaryCharacteristics";

        //Magic Numbers
        private readonly int _absoluteExpirationHours = 1;
        private readonly int _slidingExpirationFromMinutes = 5;


        //Cosntructor
        public CharacteristicRepository
            (
            DiplomaProjectContext context,
            IMemoryCache cache
            )
        {
            _cache = cache;
            _context = context;
        }


        //===============================================================================================================
        //===============================================================================================================
        //===============================================================================================================
        //Public Methods

        public async Task<IReadOnlyDictionary<int, DomainCharacteristic>> GetCharacteristicsAsync
            (
            CancellationToken cancellation
            )
        {
            var types = await GetCharacteristicTypesDictionaryAsync(cancellation);

            return types.SelectMany(x => x.Value.CharacteristicDictionary)
                .ToDictionary(x => x.Key, x => x.Value);
        }

        public async Task<IReadOnlyDictionary<int, DomainCharacteristicType>> GetCharacteristicTypesAsync
            (
            CancellationToken cancellation
            )
        {
            return await GetCharacteristicTypesDictionaryAsync(cancellation);
        }
        //===============================================================================================================
        //===============================================================================================================
        //===============================================================================================================
        //Private Methods
        private async Task<Dictionary<int, DomainCharacteristicType>> GetCharacteristicTypesDictionaryAsync
            (
            CancellationToken cancellation
            )
        {
            if (!_cache.TryGetValue
                (
                _cacheKeyCharacteristics,
                out Dictionary<int, DomainCharacteristicType>? dictionary
                )
                ||
                dictionary == null
                )
            {
                dictionary = [];
                // Jeśli nie ma danych w cache, załaduj je z bazy danych
                // < 10 Dla Jezyków
                var sharedQualities = await _context.Qualities.Where(x => x.Id > 10)
                    .Select(x => new DomainQuality
                    (
                    x.Id,
                    x.Name,
                    x.Description
                    )).ToListAsync(cancellation);

                var tupleList = await _context.CharacteristicTypes
                .Select(x => new
                {
                    Type = x,
                    Qualities = x.Qualities.Select(q => new DomainQuality(q.Id, q.Name, q.Description)),
                    Characteristics = x.Characteristics.Select(y => new
                    {
                        ParentIds = y.ParentCharacteristics.Select(z => z.Id),
                        ChildIds = y.ChildCharacteristics.Select(z => z.Id),
                        Characteristic = y
                    }).ToList()
                })
                    .ToListAsync(cancellation); ;

                foreach (var tuple in tupleList)
                {

                    var type = new DomainCharacteristicType(tuple.Type.Id, tuple.Type.Name, tuple.Type.Description);

                    var characteristics = new List<DomainCharacteristic>();
                    foreach (var c in tuple.Characteristics)
                    {
                        var ids = c.ParentIds.Union(c.ChildIds);
                        var characteristic = new DomainCharacteristic
                            (
                            c.Characteristic.Id,
                            c.Characteristic.Name,
                            c.Characteristic.Description,
                            c.Characteristic.CharacteristicTypeId,
                            ids
                            );
                        characteristics.Add(characteristic);
                    }

                    type.SetQualities(tuple.Qualities);
                    type.SetCharacteristics(characteristics);

                    dictionary[type.Id] = type;
                }

                // Przechowaj dane w cache
                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(_absoluteExpirationHours),
                    SlidingExpiration = TimeSpan.FromMinutes(_slidingExpirationFromMinutes)
                };

                _cache.Set(_cacheKeyCharacteristics, dictionary, cacheOptions);
            }
            return dictionary;
        }

    }
}
