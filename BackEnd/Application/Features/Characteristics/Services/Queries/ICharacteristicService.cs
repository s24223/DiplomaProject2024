using Application.Features.Characteristics.DTOs.Queries;
using Application.Shared.DTOs.Response;

namespace Application.Features.Characteristics.Services.Queries
{
    public interface ICharacteristicService
    {
        Task<ResponseItems<CharacteristicTypeDetailsResponseDto>> GetCharacteristicTypesAsync
            (
            CancellationToken cancellation
            );
        //Task<ResponseItems<CharacteristicTypeDetailsResponseDto>> GetCharacteristicTypesDictionaryAsync();
    }
}
