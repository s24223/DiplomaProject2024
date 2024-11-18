using Domain.Features.Characteristic.Entities;
using Domain.Features.Characteristic.ValueObjects.Identificators;

namespace Domain.Features.Characteristic.Repositories
{
    public interface ICharacteristicQueryRepository
    {
        IReadOnlyDictionary<CharacteristicId, DomainCharacteristic> GetCharDictionary();
        IReadOnlyDictionary<CharacteristicTypeId, DomainCharacteristicType> GetCharacteristicTypes();

        IReadOnlyDictionary
            <(CharacteristicId CharacteristicId, QualityId? QualityId),
            (DomainCharacteristic Characteristic, DomainQuality? Quality)>
            GetCollocations
            (
            IEnumerable<(CharacteristicId CharacteristicId, QualityId? QualityId)> values
            );
        IEnumerable<DomainCharacteristic> GetCharList(IEnumerable<CharacteristicId> ids);
    }
}
