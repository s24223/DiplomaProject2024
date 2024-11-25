namespace Application.Features.Users.Commands.Urls.DTOs
{
    public class CreateUrlReq
    {
        public required int UrlTypeId { get; init; }
        public required string Path { get; init; } = null!;
        public string? Name { get; init; } = null;
        public string? Description { get; init; } = null;
    }
}
