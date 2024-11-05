namespace Domain.Features.Characteristic.ValueObjects
{
    public record DomainCharacteristic
    {
        //Values
        public int Id { get; private set; }
        public string Name { get; private set; } = null!;
        public string Description { get; private set; } = null!;
        public IReadOnlyList<int> ConnectedIds { get; private set; } = [];

        //References
        public int CharacteristicTypeId { get; private set; }
        private DomainCharacteristicType _characteristicType = null!;


        //Cosntructor
        public DomainCharacteristic
            (
            int id,
            string name,
            string description,
            int characteristicTypeId,
            IEnumerable<int> connectedIds
            )
        {
            Id = id;
            Name = name;
            Description = description;
            CharacteristicTypeId = characteristicTypeId;
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
        //=======================================================================================================
        //=======================================================================================================
        //=======================================================================================================
        //Private Methods
    }
}
