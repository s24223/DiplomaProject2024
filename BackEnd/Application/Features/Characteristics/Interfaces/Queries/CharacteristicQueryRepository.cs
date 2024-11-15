using Application.Databases.Relational;
using Application.Features.Characteristics.Mappers.DatabaseToDomain;
using Domain.Features.Characteristic.Entities;
using Domain.Features.Characteristic.Exceptions;
using Domain.Features.Characteristic.Repositories;
using Domain.Features.Characteristic.ValueObjects.Identificators;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Application.Features.Characteristics.Interfaces.Queries
{
    public class CharacteristicQueryRepository : ICharacteristicQueryRepository
    {
        //Values
        private readonly ICharacteristicMapper _mapper;
        private readonly DiplomaProjectContext _context;
        private readonly IMemoryCache _cache;

        //Cache Settengs
        private readonly string _cacheKeyCharacteristics = "DictionaryCharacteristics";
        private readonly int _absoluteExpirationHours = 1;
        private readonly int _slidingExpirationFromMinutes = 5;


        //Cosntructor
        public CharacteristicQueryRepository
            (
            ICharacteristicMapper mapper,
            DiplomaProjectContext context,
            IMemoryCache cache
            )
        {
            _cache = cache;
            _mapper = mapper;
            _context = context;
        }


        //===============================================================================================================
        //===============================================================================================================
        //===============================================================================================================
        //Public Methods

        public IReadOnlyDictionary<CharacteristicId, DomainCharacteristic> GetCharacteristics()
        {
            var types = GetCharacteristicTypesDictionary();
            return types.SelectMany(x => x.Value.CharacteristicDictionary)
                .ToDictionary(x => x.Key, x => x.Value);
        }

        public IReadOnlyDictionary<CharacteristicTypeId, DomainCharacteristicType> GetCharacteristicTypes()
        {
            return GetCharacteristicTypesDictionary();
        }

        public IReadOnlyDictionary
            <(CharacteristicId CharacteristicId, QualityId? QualityId),
            (DomainCharacteristic Characteristic, DomainQuality? Quality)>
            GetCollocations
            (IEnumerable<(CharacteristicId CharacteristicId, QualityId? QualityId)> values)
        {
            var characteristics = GetCharacteristics();
            //Variant 1 Ignore and Set Which exist
            //+ Variant 2 Throw Exception

            var dictionary = new Dictionary
                <(CharacteristicId CharacteristicId, QualityId? QualityId),
            (DomainCharacteristic Characteristic, DomainQuality? Quality)>();


            foreach (var pair in values)
            {
                if (!characteristics.TryGetValue(pair.CharacteristicId, out var characteristic))
                {
                    throw new CharacteristicException
                        ($"{Messages.CharacteristicI_CharacteristicId_NotFound}: {pair.CharacteristicId.Value}");
                }

                DomainQuality? quality = pair.QualityId != null ?
                    characteristic.GetQuality(pair.QualityId) :
                    null;

                dictionary[pair] = (characteristic, quality);
            }
            return dictionary;
        }

        //===============================================================================================================
        //===============================================================================================================
        //===============================================================================================================
        //Private Methods
        private Dictionary<CharacteristicTypeId, DomainCharacteristicType>
            GetCharacteristicTypesDictionary()
        {
            if (_cache.TryGetValue
                (_cacheKeyCharacteristics, out Dictionary<CharacteristicTypeId, DomainCharacteristicType>? dictionary) &&
                dictionary != null
                )
            {
                return dictionary;
            }

            dictionary = MapToDomainCharacteristicTypes();
            SetToCache(dictionary);
            return dictionary;
        }

        private void SetToCache(Dictionary<CharacteristicTypeId, DomainCharacteristicType> dictionary)
        {
            //Czas Przechowywania w Cache  
            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(_absoluteExpirationHours),
                SlidingExpiration = TimeSpan.FromMinutes(_slidingExpirationFromMinutes)
            };
            _cache.Set(_cacheKeyCharacteristics, dictionary, cacheOptions);
        }

        private Dictionary<CharacteristicTypeId, DomainCharacteristicType>
            MapToDomainCharacteristicTypes()
        {
            var sharedQualities = _context.Qualities
                .AsNoTracking()
                .Where(x => x.CharacteristicTypeId == null)
                .Select(x => _mapper.DomainQuality(x))
                .ToList();

            var typesWithDetails = _context.CharacteristicTypes
                .AsNoTracking()
                .Select(x => new
                {
                    Type = _mapper.DomainCharacteristicType(x),
                    Qualities = x.Qualities.Select(q => _mapper.DomainQuality(q)),
                    Characteristics = x.Characteristics.Select(y => new
                    {
                        Characteristic = y,
                        ParentCharacteristics = y.ParentCharacteristics.Select(z => z.Id),
                        ChildCharacteristics = y.ChildCharacteristics.Select(z => z.Id)
                    })
                .ToList()
                })
            .ToList();

            var dictionary = typesWithDetails.ToDictionary(tuple => tuple.Type.Id, tuple =>
            {
                var type = tuple.Type;
                var doaminCharacteristics = tuple.Characteristics.Select(x => _mapper.DomainCharacteristic
                    (
                    x.Characteristic,
                    x.ParentCharacteristics.Concat(x.ChildCharacteristics).Distinct()
                    ));
                type.SetCharacteristics(doaminCharacteristics);

                // Ustawianie cech — wspólne cechy tylko wtedy, gdy lista jest pusta.
                type.SetQualities(tuple.Qualities.Any() ? tuple.Qualities : sharedQualities);

                return type;
            });
            return dictionary;
        }


    }
}
