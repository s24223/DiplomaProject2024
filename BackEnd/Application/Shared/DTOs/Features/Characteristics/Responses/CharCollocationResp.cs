using Domain.Features.Characteristic.Entities;

namespace Application.Shared.DTOs.Features.Characteristics.Responses
{
    public class CharCollocationResp
    {
        //Values
        public CharResp Characteristic { get; set; } = null!;
        public CharTypeResp CharacteristicType { get; set; } = null!;
        public QualityResp? Quality { get; set; } = null;


        //Constructor 
        public CharCollocationResp
            (
            DomainCharacteristic characteristic,
            DomainQuality? quality
            )
        {
            Characteristic = new CharResp(characteristic);
            Quality = quality == null ? null : new QualityResp(quality);

            if (characteristic.CharacteristicType != null)
            {
                CharacteristicType = new CharTypeResp
                    (characteristic.CharacteristicType);
            }
        }
    }
}
