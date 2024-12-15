using Application.Features.Characteristics.Queries.DTOs;
using Application.Shared.DTOs.Response;
using Domain.Features.Characteristic.Repositories;

namespace Application.Features.Characteristics.Queries.Services
{
    public class CharacteristicQuerySvc : ICharacteristicQuerySvc
    {
        //Values
        private readonly ICharacteristicQueryRepository _repository;


        //Constructor
        public CharacteristicQuerySvc(ICharacteristicQueryRepository repository)
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
            var dictionary = _repository.GetCharDictionary();
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
