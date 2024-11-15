using Domain.Features.Characteristic.Entities;

namespace Application.Shared.DTOs.Features.Characteristics.Responses
{
    public class CharacteristicCollocationResponseDto
    {
        //Values
        public CharacteristicResponseDto Characteristic { get; set; } = null!;
        public CharacteristicTypeResponseDto CharacteristicType { get; set; } = null!;
        public QualityResponseDto? Quality { get; set; } = null;


        //Constructor 
        public CharacteristicCollocationResponseDto
            (
            DomainCharacteristic characteristic,
            DomainQuality? quality
            )
        {
            Characteristic = new CharacteristicResponseDto(characteristic);
            Quality = quality == null ? null : new QualityResponseDto(quality);

            if (characteristic.CharacteristicType != null)
            {
                CharacteristicType = new CharacteristicTypeResponseDto
                    (characteristic.CharacteristicType);
            }
        }
    }
}
