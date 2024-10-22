using Domain.Features.Url.Entities;

namespace Application.Features.User.DTOs.CommandsUser
{
    public class UrlResponseDto
    {
        public Guid UserId { get; set; }
        public DateTime Created { get; set; }
        public int UrlTypeId { get; set; }
        public string UrlType { get; set; }
        public string UrlTypeDescription { get; set; }
        public string Path { get; set; } = null!;
        public string? Name { get; set; }
        public string? Description { get; set; }

        public UrlResponseDto(DomainUrl url)
        {
            UserId = url.Id.UserId.Value;
            UrlTypeId = (int)url.Id.UrlTypeId;
            UrlType = url.Type.Name;
            UrlTypeDescription = url.Type.Description;
            Created = url.Id.Created;
            Path = url.Path.ToString();
            Name = url.Name;
            Description = url.Description;
        }
    }
}
