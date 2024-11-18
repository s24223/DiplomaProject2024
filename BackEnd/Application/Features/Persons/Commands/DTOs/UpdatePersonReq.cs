using Application.Shared.DTOs;
using Application.Shared.DTOs.Features.Characteristics.Requests;

namespace Application.Features.Persons.Commands.DTOs
{
    public class UpdatePersonReq
    {
        public string? UrlSegment { get; set; }
        public required string ContactEmail { get; set; } = null!;
        public required string Name { get; set; } = null!;
        public required string Surname { get; set; } = null!;
        public DateOnlyRequestDto? BirthDate { get; set; }
        public string? ContactPhoneNum { get; set; }
        public string? Description { get; set; }
        public required bool IsStudent { get; set; }
        public required bool IsPublicProfile { get; set; }
        public Guid? AddressId { get; set; }
        public IEnumerable<CharacteristicCollocationRequestDto> Characteristics { get; set; } = [];
    }
}
