namespace Application.Features.Users.Commands.Urls.DTOs.Update
{
    public class UpdateUrlRequestDto
    {
        public required int UrlTypeId { get; set; }
        public required DateTime Created { get; set; }
        public required UpdateUrlDataRequestDto UpdateData { get; set; }
    }
}
