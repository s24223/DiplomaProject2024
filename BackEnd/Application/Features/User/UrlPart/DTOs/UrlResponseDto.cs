namespace Application.Features.User.UrlPart.DTOs
{
    public class UrlResponseDto
    {
        public required Guid UserId { get; set; }
        public required DateTime Created { get; set; }
        public required int UrlTypeId { get; set; }
        public required string UrlType { get; set; }
        public required string UrlTypeDescription { get; set; }
        public required string Path { get; set; } = null!;
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}
