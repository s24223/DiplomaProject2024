using Domain.Features.Characteristic.ValueObjects.Identificators;
using System.ComponentModel.DataAnnotations;

namespace Application.Shared.DTOs.Features.Characteristics.Requests
{
    public class CharacteristicCollocationRequestDto
    {
        [Required]
        public int CharacteristicId { get; set; }
        public int? QualityId { get; set; } = null;


        //=======================================================================================
        //=======================================================================================
        //=======================================================================================
        //Methods

        public static implicit operator (CharacteristicId, QualityId?)
            (CharacteristicCollocationRequestDto dto)
        {
            var characteristicId = new CharacteristicId(dto.CharacteristicId);
            var qualityId = dto.QualityId.HasValue ? new QualityId(dto.QualityId.Value) : null;
            return (characteristicId, qualityId);
        }
    }
}
