using Application.Shared.DTOs;
using Application.Shared.DTOs.Features.Characteristics.Requests;

namespace Application.Features.Persons.Commands.DTOs
{
    public class UpdatePersonReq
    {
        public string? UrlSegment { get; init; }
        public required string ContactEmail { get; init; } = null!;
        public required string Name { get; init; } = null!;
        public required string Surname { get; init; } = null!;
        public DateOnlyRequestDto? BirthDate { get; init; }
        public string? ContactPhoneNum { get; init; }
        public string? Description { get; init; }
        public required bool IsStudent { get; init; }
        public required bool IsPublicProfile { get; init; }
        public Guid? AddressId { get; init; }
        public IEnumerable<CharacteristicCollocationRequestDto> Characteristics { get; init; } = [];
    }
}
