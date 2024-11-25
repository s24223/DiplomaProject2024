namespace Application.Features.Users.Commands.Urls.DTOs.Update
{
    public class UpdateUrlReq
    {
        public required int UrlTypeId { get; init; }
        public required DateTime Created { get; init; }
        public required UpdateUrlData Data { get; init; }
    }
}
