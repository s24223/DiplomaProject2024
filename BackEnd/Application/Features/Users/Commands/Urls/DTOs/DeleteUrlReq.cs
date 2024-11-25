namespace Application.Features.Users.Commands.Urls.DTOs
{
    public class DeleteUrlReq
    {
        public required int UrlTypeId { get; init; }
        public required DateTime Created { get; init; }
    }
}
