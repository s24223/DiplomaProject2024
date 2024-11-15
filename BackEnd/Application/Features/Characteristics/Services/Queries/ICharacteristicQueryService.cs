using Application.Features.Characteristics.DTOs.Queries;
using Application.Shared.DTOs.Response;

namespace Application.Features.Characteristics.Services.Queries
{
    public interface ICharacteristicQueryService
    {
        ResponseItems<GetCharacteristicTypeResponseDto> GetCharacteristicTypes();
        ResponseItems<GetCharacteristicResponseDto> GetCharacteristics();
    }
}
