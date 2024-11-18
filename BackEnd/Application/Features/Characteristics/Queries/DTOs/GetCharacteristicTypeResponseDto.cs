using Application.Shared.DTOs.Features.Characteristics.Responses;
using Domain.Features.Characteristic.Entities;

namespace Application.Features.Characteristics.Queries.DTOs
{
    public class GetCharacteristicTypeResponseDto
    {
        //Values
        public CharTypeResp CharacteristicType { get; set; } = null!;
        public IEnumerable<QualityResp> PossibleQualities { get; set; } = [];
        public IEnumerable<CharResp> Characteristics { get; set; } = [];


        //Constructor
        public GetCharacteristicTypeResponseDto(DomainCharacteristicType domain)
        {
            CharacteristicType = new CharTypeResp(domain);
            PossibleQualities = domain.QualityDictionary.Values
                .Select(x => new QualityResp(x));
            Characteristics = domain.CharacteristicDictionary.Values
                .Select(x => new CharResp(x));
        }

    }
}
