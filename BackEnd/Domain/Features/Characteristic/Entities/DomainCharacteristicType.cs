using Domain.Features.Characteristic.ValueObjects.Identificators;
using Domain.Shared.Providers;
using Domain.Shared.Templates.Entities;

namespace Domain.Features.Characteristic.Entities
{
    public class DomainCharacteristicType : Entity<CharacteristicTypeId>
    {
        //Values
        public string Name { get; private set; } = null!;
        public string Description { get; private set; } = null!;


        //References
        private Dictionary<CharacteristicId, DomainCharacteristic> _characteristicDictionary = [];
        public IReadOnlyDictionary<CharacteristicId, DomainCharacteristic> CharacteristicDictionary => _characteristicDictionary;

        private Dictionary<QualityId, DomainQuality> _qualityDictionary = [];
        public IReadOnlyDictionary<QualityId, DomainQuality> QualityDictionary => _qualityDictionary;


        //Cosntructor
        public DomainCharacteristicType
            (
            int id,
            string name,
            string description,
            IProvider provider
            ) : base(new CharacteristicTypeId(id), provider)
        {
            Name = name;
            Description = description;
        }


        //=======================================================================================================
        //=======================================================================================================
        //=======================================================================================================
        //Public Methods
        public void SetCharacteristics(IEnumerable<DomainCharacteristic> list)
        {
            foreach (var characteristic in list)
            {
                SetCharacteristic(characteristic);
            }
        }

        public void SetQualities(IEnumerable<DomainQuality> qualities)
        {
            foreach (var quality in qualities)
            {
                SetQuality(quality);
            }
        }

        //=======================================================================================================
        //=======================================================================================================
        //=======================================================================================================
        //Private Methods
        private void SetCharacteristic(DomainCharacteristic characteristic)
        {
            if (characteristic.CharacteristicTypeId == Id)
            {
                _characteristicDictionary[characteristic.Id] = characteristic;
                characteristic.CharacteristicType = this;
            }
        }

        private void SetQuality(DomainQuality quality)
        {
            _qualityDictionary[quality.Id] = quality;
        }
    }
}
