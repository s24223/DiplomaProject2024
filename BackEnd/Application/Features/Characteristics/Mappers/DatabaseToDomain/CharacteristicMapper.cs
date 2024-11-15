using Application.Databases.Relational.Models;
using Domain.Features.Characteristic.Entities;
using Domain.Features.Characteristic.Factories;

namespace Application.Features.Characteristics.Mappers.DatabaseToDomain
{
    public class CharacteristicMapper : ICharacteristicMapper
    {
        //Values
        private readonly ICharacteristicFactory _factory;


        //Cosntructor
        public CharacteristicMapper
            (
            ICharacteristicFactory factory
            )
        {
            _factory = factory;
        }


        //========================================================================================
        //========================================================================================
        //========================================================================================
        //Public Methods
        public DomainCharacteristicType DomainCharacteristicType
           (
            CharacteristicType database
           )
        {
            return _factory.DomainCharacteristicType
                (
                database.Id,
                database.Name,
                database.Description
                );
        }

        public DomainCharacteristic DomainCharacteristic
          (
          Characteristic database,
          IEnumerable<int> connectedIds
          )
        {
            return _factory.DomainCharacteristic
                (
                database.Id,
                database.Name,
                database.Description,
                database.CharacteristicTypeId,
                connectedIds
                );
        }

        public DomainQuality DomainQuality
            (
            Quality database
            )
        {
            return _factory.DomainQuality
                (
                database.Id,
                database.Name,
                database.Description
                );
        }

        //========================================================================================
        //========================================================================================
        //========================================================================================
        //Private Methods
    }
}
