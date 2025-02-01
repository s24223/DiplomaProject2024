using Domain.Features.Characteristic.Entities;
using Domain.Features.Characteristic.Repositories;
using Domain.Features.Characteristic.ValueObjects.Identificators;
using Domain.Shared.Providers;
using Domain.Shared.Providers.ExceptionMessage;

namespace DomainTests.Fakes
{
    public class CharacteristicsFake : ICharacteristicQueryRepository
    {
        //Properties
        private readonly IProvider provider = new Provider(
                    new ExceptionMessageProvider(),
                    new Domain.Shared.Providers.Time.TimeProvider());


        //==============================================================================================
        //==============================================================================================
        //==============================================================================================
        //Fake Methods
        public IReadOnlyDictionary<CharacteristicTypeId, DomainCharacteristicType> GetCharacteristicTypes()
        {
            return new Dictionary<CharacteristicTypeId, DomainCharacteristicType>
            {
                { new CharacteristicTypeId(1), new DomainCharacteristicType(1,"First", "", provider)  },
                { new CharacteristicTypeId(2), new DomainCharacteristicType(1,"Hand", "", provider)  },
            };
        }

        public IReadOnlyDictionary<CharacteristicId, DomainCharacteristic> GetCharDictionary()
        {
            return new Dictionary<CharacteristicId, DomainCharacteristic>
            {
                {
                    new CharacteristicId(1),
                    new DomainCharacteristic(1, "First", "Desc", 1,[], provider)
                },
                {
                    new CharacteristicId(2),
                    new DomainCharacteristic(2, "Hand", "Desc", 2,[], provider)
                },
            };
        }

        public IEnumerable<DomainCharacteristic> GetCharList(IEnumerable<CharacteristicId> ids)
        {
            var list = GetCharDictionary()
                .Where(pair => ids.Contains(pair.Key))
                .Select(pair => pair.Value);
            return list;
        }

        public IReadOnlyDictionary<
            (CharacteristicId CharacteristicId, QualityId? QualityId),
            (DomainCharacteristic Characteristic, DomainQuality? Quality)>
            GetCollocations(IEnumerable<(
                CharacteristicId CharacteristicId, QualityId? QualityId)> values
             )
        {
            var qualityDictionary = new Dictionary<QualityId, DomainQuality>
            {
                { new QualityId(1), new DomainQuality(1, "First", "Desc", provider) },
                { new QualityId(2), new DomainQuality(2, "Hand", "Desc", provider) }
            };

            var characteristicsDictionary = GetCharDictionary();
            var result = new Dictionary<(CharacteristicId CharacteristicId, QualityId? QualityId),
            (DomainCharacteristic Characteristic, DomainQuality? Quality)>();

            foreach (var pair in values)
            {
                if (characteristicsDictionary.TryGetValue(pair.CharacteristicId, out var characteristic))
                {
                    if (pair.QualityId != null && qualityDictionary.TryGetValue(pair.QualityId, out var quality))
                    {
                        result.Add(pair, (characteristic, quality));
                    }
                    else
                    {
                        result.Add(pair, (characteristic, null));
                    }
                }
            }
            return result;
        }
    }
}
