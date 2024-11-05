using Domain.Features.Characteristic.ValueObjects;

namespace Application.Features.Characteristics.Interfaces.Queries
{
    public interface ICharacteristicRepository
    {
        Task<IReadOnlyDictionary<int, DomainCharacteristic>> GetCharacteristicsAsync
            (
            CancellationToken cancellation
            );

        Task<IReadOnlyDictionary<int, DomainCharacteristicType>> GetCharacteristicTypesAsync
            (
            CancellationToken cancellation
            );
    }
}
