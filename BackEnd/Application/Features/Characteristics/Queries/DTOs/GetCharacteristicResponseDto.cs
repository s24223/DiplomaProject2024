using Application.Shared.DTOs.Features.Characteristics.Responses;
using Domain.Features.Characteristic.Entities;

namespace Application.Features.Characteristics.Queries.DTOs
{
    public class GetCharacteristicResponseDto
    {
        //Values
        public CharResp Characteristic { get; set; } = null!;
        public CharTypeResp CharacteristicType { get; set; } = null!;
        public IEnumerable<QualityResp> PossibleQualities { get; set; } = [];


        //Constructor
        public GetCharacteristicResponseDto(DomainCharacteristic domain)
        {
            Characteristic = new CharResp(domain);
            if (domain.CharacteristicType != null)
            {
                CharacteristicType = new CharTypeResp(domain.CharacteristicType);

                Console.WriteLine(domain.CharacteristicType.QualityDictionary.Count());
                if (domain.CharacteristicType.QualityDictionary.Any())
                {
                    PossibleQualities = domain.CharacteristicType.QualityDictionary.Values
                        .Select(x => new QualityResp(x));
                }
            }
        }
        //=================================================================================================
        //=================================================================================================
        //=================================================================================================
    }
}
