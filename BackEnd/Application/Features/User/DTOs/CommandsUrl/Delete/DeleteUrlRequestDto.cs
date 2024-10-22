namespace Application.Features.User.DTOs.CommandsUrl.Delete
{
    public class DeleteUrlRequestDto
    {
        public required int UrlTypeId { get; set; }
        public required DateTime Created { get; set; }
    }
}
