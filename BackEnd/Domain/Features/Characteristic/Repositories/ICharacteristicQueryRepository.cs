using Domain.Features.Characteristic.Entities;
using Domain.Features.Characteristic.ValueObjects.Identificators;

namespace Domain.Features.Characteristic.Repositories
{
    public interface ICharacteristicQueryRepository
    {
        IReadOnlyDictionary<CharacteristicId, DomainCharacteristic> GetCharacteristics();
        IReadOnlyDictionary<CharacteristicTypeId, DomainCharacteristicType> GetCharacteristicTypes();

        IReadOnlyDictionary
            <(CharacteristicId CharacteristicId, QualityId? QualityId),
            (DomainCharacteristic Characteristic, DomainQuality? Quality)>
            GetCollocations
            (IEnumerable<(CharacteristicId CharacteristicId, QualityId? QualityId)> values);
    }
}
