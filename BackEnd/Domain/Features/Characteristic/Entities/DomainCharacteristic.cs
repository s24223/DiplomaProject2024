using Domain.Features.Characteristic.Exceptions;
using Domain.Features.Characteristic.ValueObjects.Identificators;
using Domain.Shared.Providers;
using Domain.Shared.Templates.Entities;

namespace Domain.Features.Characteristic.Entities
{
    public class DomainCharacteristic : Entity<CharacteristicId>
    {
        //Values
        public string Name { get; private set; } = null!;
        public string Description { get; private set; } = null!;
        public IReadOnlyList<int> ConnectedIds { get; private set; } = [];

        //References
        public CharacteristicTypeId CharacteristicTypeId { get; private set; }
        private DomainCharacteristicType _characteristicType = null!;


        //Cosntructor
        public DomainCharacteristic
            (
            int id,
            string name,
            string description,
            int characteristicTypeId,
            IEnumerable<int> connectedIds,
            IProvider provider
            ) : base(new CharacteristicId(id), provider)
        {
            Name = name;
            Description = description;
            CharacteristicTypeId = new CharacteristicTypeId(characteristicTypeId);
            ConnectedIds = connectedIds.ToList();
        }


        //=======================================================================================================
        //=======================================================================================================
        //=======================================================================================================
        //Public Methods
        public DomainCharacteristicType CharacteristicType
        {
            get { return _characteristicType; }
            set
            {
                if (_characteristicType == null && value != null && CharacteristicTypeId == value.Id)
                {
                    _characteristicType = value;
                    value.SetCharacteristics([this]);
                }
            }
        }

        public DomainQuality GetQuality(QualityId id)
        {
            if (!_characteristicType.QualityDictionary.TryGetValue(id, out var domain))
            {
                throw new QualityException($"NotExist Quality by Id: {id.Value}");
            }
            return domain;
        }
        //=======================================================================================================
        //=======================================================================================================
        //=======================================================================================================
        //Private Methods
    }
}
