using System.ComponentModel.DataAnnotations;

namespace Application.Features.Users.Commands.Notifications.DTOs.Create
{
    public class CreateUnAuthNotificationReq
    {
        [Required]
        [EmailAddress]
        public required string Email { get; init; }
        public Guid? PreviousProblemId { get; init; }
        public Guid? IdAppProblem { get; init; }
        [Required]
        public required string UserMessage { get; init; } = null!;
    }
}
