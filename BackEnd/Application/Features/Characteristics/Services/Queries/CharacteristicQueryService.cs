using Application.Features.Characteristics.DTOs.Queries;
using Application.Shared.DTOs.Response;
using Domain.Features.Characteristic.Repositories;

namespace Application.Features.Characteristics.Services.Queries
{
    public class CharacteristicQueryService : ICharacteristicQueryService
    {
        //Values
        private readonly ICharacteristicQueryRepository _repository;

        //Cosntructor
        public CharacteristicQueryService
            (
            ICharacteristicQueryRepository repository
            )
        {
            _repository = repository;
        }



        //=================================================================================================
        //=================================================================================================
        //=================================================================================================
        //Public Methods
        public ResponseItems<GetCharacteristicTypeResponseDto> GetCharacteristicTypes()
        {
            var dictionary = _repository.GetCharacteristicTypes();
            return new ResponseItems<GetCharacteristicTypeResponseDto>
            {
                Items = dictionary.Values
                    .Select(x => new GetCharacteristicTypeResponseDto(x))
                    .ToList(),
            };
        }

        public ResponseItems<GetCharacteristicResponseDto> GetCharacteristics()
        {
            var dictionary = _repository.GetCharacteristics();
            return new ResponseItems<GetCharacteristicResponseDto>
            {
                Items = dictionary.Values
                    .Select(x => new GetCharacteristicResponseDto(x))
                    .ToList()
            };
        }

        //=================================================================================================
        //=================================================================================================
        //=================================================================================================
        //Private Methods
    }
}
