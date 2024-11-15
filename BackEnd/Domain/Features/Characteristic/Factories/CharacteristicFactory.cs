using Domain.Features.Characteristic.Entities;
using Domain.Shared.Providers;

namespace Domain.Features.Characteristic.Factories
{
    public class CharacteristicFactory : ICharacteristicFactory
    {
        //Values
        private readonly IProvider _provider;


        //Constructor
        public CharacteristicFactory
            (
            IProvider provider
            )
        {
            _provider = provider;
        }


        //==================================================================================================
        //==================================================================================================
        //==================================================================================================
        //Public Methods
        public DomainCharacteristicType DomainCharacteristicType
            (
            int id,
            string name,
            string description
            )
        {
            return new DomainCharacteristicType
                (
                id,
                name,
                description,
                _provider
                );
        }
        public DomainCharacteristic DomainCharacteristic
            (
            int id,
            string name,
            string description,
            int characteristicTypeId,
            IEnumerable<int> connectedIds
            )
        {
            return new DomainCharacteristic
                (
                id,
                name,
                description,
                characteristicTypeId,
                connectedIds,
                _provider
                );
        }
        public DomainQuality DomainQuality
            (
            int id,
            string name,
            string description
            )
        {
            return new DomainQuality
                (
                id,
                name,
                description,
                _provider
                );
        }
        //==================================================================================================
        //==================================================================================================
        //==================================================================================================
        //Private Methods

    }
}
