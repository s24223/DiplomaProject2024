using Domain.Features.Characteristic.Entities;

namespace Domain.Features.Characteristic.Factories
{
    public interface ICharacteristicFactory
    {
        DomainCharacteristicType DomainCharacteristicType
           (
           int id,
           string name,
           string description
           );
        DomainCharacteristic DomainCharacteristic
          (
          int id,
          string name,
          string description,
          int characteristicTypeId,
          IEnumerable<int> connectedIds
          );

        DomainQuality DomainQuality
            (
            int id,
            string name,
            string description
            );
    }
}
