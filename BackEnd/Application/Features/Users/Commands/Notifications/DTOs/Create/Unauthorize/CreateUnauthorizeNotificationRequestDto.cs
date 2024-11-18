namespace Application.Features.Users.Commands.Notifications.DTOs.Create.Unauthorize
{
    public class CreateUnauthorizeNotificationRequestDto
    {
        public required string Email { get; set; }
        public Guid? PreviousProblemId { get; set; }
        public Guid? IdAppProblem { get; set; }
        public required string UserMessage { get; set; } = null!;
    }
}
