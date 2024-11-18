using Application.Databases.Relational.Models;
using Domain.Features.Characteristic.Entities;

namespace Application.Features.Characteristics.Mappers
{
    public interface ICharacteristicMapper
    {
        DomainCharacteristicType DomainCharacteristicType
           (
            CharacteristicType database
           );

        DomainCharacteristic DomainCharacteristic
          (
          Characteristic database,
          IEnumerable<int> connectedIds
          );

        DomainQuality DomainQuality
            (
            Quality database
            );
    }
}
