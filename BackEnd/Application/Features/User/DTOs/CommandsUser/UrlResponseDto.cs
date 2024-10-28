using Application.Shared.DTOs.Features.Users.Urls;
using Domain.Features.Url.Entities;

namespace Application.Features.User.DTOs.CommandsUser
{
    public class UrlResponseDto
    {
        public Guid UserId { get; set; }
        public DateTime Created { get; set; }
        public int UrlTypeId { get; set; }
        public string Path { get; set; } = null!;
        public string? Name { get; set; }
        public string? Description { get; set; }
        public UrlTypeResponseDto Type { get; set; }


        public UrlResponseDto(DomainUrl url)
        {
            UserId = url.Id.UserId.Value;
            UrlTypeId = url.Id.UrlTypeId;
            Created = url.Id.Created;
            Path = url.Path.ToString();
            Name = url.Name;
            Description = url.Description;
            Type = new UrlTypeResponseDto
            {
                Id = url.Type.Id,
                Name = url.Type.Name,
                Description = url.Type.Description,
            };
        }
    }
}
