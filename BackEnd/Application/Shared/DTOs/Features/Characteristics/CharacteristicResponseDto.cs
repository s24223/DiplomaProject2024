using Domain.Features.Characteristic.ValueObjects;

namespace Application.Shared.DTOs.Features.Characteristics
{
    public class CharacteristicResponseDto
    {
        //Values
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int CharacteristicTypeId { get; set; }
        public IEnumerable<int> ConnectedIds { get; set; } = [];

        //Cosntructor
        public CharacteristicResponseDto(DomainCharacteristic domain)
        {
            Id = domain.Id;
            Name = domain.Name;
            Description = domain.Description;
            CharacteristicTypeId = domain.CharacteristicTypeId;
            ConnectedIds = domain.ConnectedIds;
        }
    }
}
