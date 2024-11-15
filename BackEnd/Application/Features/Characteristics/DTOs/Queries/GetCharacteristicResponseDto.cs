using Application.Shared.DTOs.Features.Characteristics.Responses;
using Domain.Features.Characteristic.Entities;

namespace Application.Features.Characteristics.DTOs.Queries
{
    public class GetCharacteristicResponseDto
    {
        //Values
        public CharacteristicResponseDto Characteristic { get; set; } = null!;
        public CharacteristicTypeResponseDto CharacteristicType { get; set; } = null!;
        public IEnumerable<QualityResponseDto> PossibleQualities { get; set; } = [];


        //Constructor
        public GetCharacteristicResponseDto(DomainCharacteristic domain)
        {
            Characteristic = new CharacteristicResponseDto(domain);
            if (domain.CharacteristicType != null)
            {
                CharacteristicType = new CharacteristicTypeResponseDto(domain.CharacteristicType);

                Console.WriteLine(domain.CharacteristicType.QualityDictionary.Count());
                if (domain.CharacteristicType.QualityDictionary.Any())
                {
                    PossibleQualities = domain.CharacteristicType.QualityDictionary.Values
                        .Select(x => new QualityResponseDto(x));
                }
            }
        }
    }
}
