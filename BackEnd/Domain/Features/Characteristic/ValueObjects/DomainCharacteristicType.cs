namespace Domain.Features.Characteristic.ValueObjects
{
    public record DomainCharacteristicType
    {
        //Values
        public int Id { get; private set; }
        public string Name { get; private set; } = null!;
        public string Description { get; private set; } = null!;


        //References
        private Dictionary<int, DomainCharacteristic> _characteristicDictionary = [];
        public IReadOnlyDictionary<int, DomainCharacteristic> CharacteristicDictionary => _characteristicDictionary;

        private Dictionary<int, DomainQuality> _qualityDictionary = [];
        public IReadOnlyDictionary<int, DomainQuality> QualityDictionary => _qualityDictionary;


        //Cosntructor
        public DomainCharacteristicType
            (
            int id,
            string name,
            string description
            )
        {
            Id = id;
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
            }
        }

        private void SetQuality(DomainQuality quality)
        {
            _qualityDictionary[quality.Id] = quality;
        }
    }
}
