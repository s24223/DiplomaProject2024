using Domain.Features.Characteristic.Entities;

namespace Application.Shared.DTOs.Features.Characteristics.Responses
{
    public class CharResp
    {
        //Values
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int CharacteristicTypeId { get; set; }
        public IEnumerable<int> ConnectedIds { get; set; } = [];


        //Cosntructor
        public CharResp(DomainCharacteristic domain)
        {
            Id = domain.Id.Value;
            Name = domain.Name;
            Description = domain.Description;
            CharacteristicTypeId = domain.CharacteristicTypeId.Value;
            ConnectedIds = domain.ConnectedIds;
        }
    }
}
