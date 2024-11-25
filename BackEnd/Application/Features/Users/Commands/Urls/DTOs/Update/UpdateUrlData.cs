namespace Application.Features.Users.Commands.Urls.DTOs.Update
{
    public class UpdateUrlData
    {
        public required string Path { get; init; } = null!;
        public string? Name { get; init; } = null;
        public string? Description { get; init; } = null;
    }
}
