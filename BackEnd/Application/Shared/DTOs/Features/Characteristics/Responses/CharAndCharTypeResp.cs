using Domain.Features.Characteristic.Entities;

namespace Application.Shared.DTOs.Features.Characteristics.Responses
{
    public class CharAndCharTypeResp
    {
        //Values
        public CharResp Characteristic { get; private set; } = null!;
        public CharTypeResp Type { get; private set; } = null!;


        //Values
        public CharAndCharTypeResp(DomainCharacteristic domain)
        {
            Characteristic = new CharResp(domain);
            if (domain.CharacteristicType != null)
            {
                Type = new CharTypeResp(domain.CharacteristicType);
            }
        }
    }
}
