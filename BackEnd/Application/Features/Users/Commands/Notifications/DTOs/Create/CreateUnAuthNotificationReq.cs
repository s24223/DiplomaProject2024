namespace Application.Features.Users.Commands.Notifications.DTOs.Create
{
    public class CreateUnAuthNotificationReq
    {
        public required string Email { get; init; }
        public Guid? PreviousProblemId { get; init; }
        public Guid? IdAppProblem { get; init; }
        public required string UserMessage { get; init; } = null!;
    }
}
