using Application.Shared.Exceptions;
using System.ComponentModel.DataAnnotations;

namespace Application.Shared.DTOs
{
    public class DateOnlyRequestDto
    {
        [Required]
        [Range(1900, 9999, ErrorMessage = "Wartość musi być pomiędzy {1} a {2}.")]
        public int Year { get; set; }
        [Required]
        [Range(1, 12, ErrorMessage = "Wartość musi być pomiędzy 1 a 12.")]
        public int Month { get; set; }
        [Required]
        [Range(1, 31, ErrorMessage = "Wartość musi być pomiędzy 1 a 31.")]
        public int Day { get; set; }


        //====================================================================================================
        //====================================================================================================
        //====================================================================================================
        //Methods
        public static implicit operator DateOnly(DateOnlyRequestDto dto)
        {
            try
            {
                return new DateOnly(dto.Year, dto.Month, dto.Day);
            }
            catch (Exception)
            {
                throw new DateOnlyException($"{Messages.DateOnly_ValueAttribute_InvalidDate}: {dto.Year}-{dto.Month}-{dto.Day}");
            }
        }

        public static implicit operator DateOnly?(DateOnlyRequestDto? dto)
        {
            return dto is null ? null : (DateOnly)dto;
        }
    }
}
