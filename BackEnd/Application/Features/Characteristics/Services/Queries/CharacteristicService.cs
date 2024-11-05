using Application.Features.Characteristics.DTOs.Queries;
using Application.Features.Characteristics.Interfaces.Queries;
using Application.Shared.DTOs.Response;

namespace Application.Features.Characteristics.Services.Queries
{
    public class CharacteristicService : ICharacteristicService
    {
        //Values
        private readonly ICharacteristicRepository _repository;

        //Cosntructor
        public CharacteristicService
            (
            ICharacteristicRepository repository
            )
        {
            _repository = repository;
        }


        //=================================================================================================
        //=================================================================================================
        //=================================================================================================
        //Public Methods
        public async Task<ResponseItems<CharacteristicTypeDetailsResponseDto>> GetCharacteristicTypesAsync
            (
            CancellationToken cancellation
            )
        {
            var dictionary = await _repository.GetCharacteristicTypesAsync(cancellation);
            return new ResponseItems<CharacteristicTypeDetailsResponseDto>
            {
                Items = dictionary.Values
                    .Select(x => new CharacteristicTypeDetailsResponseDto(x))
                    .ToList(),
            };
        }

        //=================================================================================================
        //=================================================================================================
        //=================================================================================================
        //Private Methods
    }
}
