using Domain.Features.Url.ValueObjects.UrlTypePart;

namespace Application.Features.User.DTOs.CommandsUrl
{
    public class UrlTypeResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;

        public UrlTypeResponseDto(UrlType type)
        {
            Id = (int)type.Type;
            Name = type.Name;
            Description = type.Description;
        }
    }
}
