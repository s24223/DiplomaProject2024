using Application.Shared.DTOs.Features.Characteristics;
using Domain.Features.Characteristic.ValueObjects;

namespace Application.Features.Characteristics.DTOs.Queries
{
    public class CharacteristicTypeDetailsResponseDto
    {
        //Values
        public CharacteristicTypeResponseDto CharacteristicType { get; set; } = null!;
        public IEnumerable<QualityResponseDto> Qualities { get; set; } = [];
        public IEnumerable<CharacteristicResponseDto> Characteristics { get; set; } = [];


        //Constructor
        public CharacteristicTypeDetailsResponseDto(DomainCharacteristicType domain)
        {
            CharacteristicType = new CharacteristicTypeResponseDto(domain);
            Qualities = domain.QualityDictionary.Values
                .Select(x => new QualityResponseDto(x));
            Characteristics = domain.CharacteristicDictionary.Values
                .Select(x => new CharacteristicResponseDto(x));
        }

    }
}
