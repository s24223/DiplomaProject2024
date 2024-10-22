namespace Application.Features.User.DTOs.CommandsUrl.Update
{
    public class UpdateUrlDataRequestDto
    {
        public required string Path { get; set; } = null!;
        public string? Name { get; set; } = null;
        public string? Description { get; set; } = null;
    }
}
