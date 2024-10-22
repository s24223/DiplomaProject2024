using Domain.Features.Url.Entities;

namespace Application.Shared.DTOs.Urls
{
    public class UrlResponseDto
    {
        public Guid UserId { get; set; }
        public int UrlTypeId { get; set; }
        public DateTime Created { get; set; }
        public string Path { get; private set; } = null!;
        public string? Name { get; private set; }
        public string? Description { get; private set; }
        public UrlTypeResponseDto Type { get; private set; } = null!;


        //Cosntructor
        public UrlResponseDto(DomainUrl url)
        {
            UserId = url.Id.UserId.Value;
            UrlTypeId = url.Id.UrlTypeId;
            Created = url.Id.Created;
            Path = url.Path;
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
