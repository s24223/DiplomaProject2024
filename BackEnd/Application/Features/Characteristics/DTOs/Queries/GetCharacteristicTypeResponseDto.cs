using Application.Shared.DTOs.Features.Characteristics.Responses;
using Domain.Features.Characteristic.Entities;

namespace Application.Features.Characteristics.DTOs.Queries
{
    public class GetCharacteristicTypeResponseDto
    {
        //Values
        public CharacteristicTypeResponseDto CharacteristicType { get; set; } = null!;
        public IEnumerable<QualityResponseDto> PossibleQualities { get; set; } = [];
        public IEnumerable<CharacteristicResponseDto> Characteristics { get; set; } = [];


        //Constructor
        public GetCharacteristicTypeResponseDto(DomainCharacteristicType domain)
        {
            CharacteristicType = new CharacteristicTypeResponseDto(domain);
            PossibleQualities = domain.QualityDictionary.Values
                .Select(x => new QualityResponseDto(x));
            Characteristics = domain.CharacteristicDictionary.Values
                .Select(x => new CharacteristicResponseDto(x));
        }

    }
}
