using Application.Features.Characteristics.Queries.DTOs;
using Application.Shared.DTOs.Response;

namespace Application.Features.Characteristics.Queries.Services
{
    public interface ICharacteristicQueryService
    {
        ResponseItems<GetCharacteristicTypeResponseDto> GetCharacteristicTypes();
        ResponseItems<GetCharacteristicResponseDto> GetCharacteristics();
    }
}
