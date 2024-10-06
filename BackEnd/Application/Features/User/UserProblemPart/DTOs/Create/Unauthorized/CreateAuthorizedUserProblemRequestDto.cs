namespace Application.Features.User.UserProblemPart.DTOs.Create.Unauthorized
{
    public class CreateAuthorizedUserProblemRequestDto
    {
        public required string UserMessage { get; set; } = null!;
        public Guid? PreviousProblemId { get; set; }
    }
}
