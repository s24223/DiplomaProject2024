namespace Application.Features.Users.Commands.Urls.DTOs.Delete
{
    public class DeleteUrlRequestDto
    {
        public required int UrlTypeId { get; set; }
        public required DateTime Created { get; set; }
    }
}
