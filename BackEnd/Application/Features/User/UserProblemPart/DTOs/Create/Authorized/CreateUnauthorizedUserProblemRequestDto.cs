using System.ComponentModel.DataAnnotations;

namespace Application.Features.User.UserProblemPart.DTOs.Create.Authorized
{
    public class CreateUnauthorizedUserProblemRequestDto
    {
        [Required]
        public required string UserMessage { get; set; } = null!;
        [Required]
        [EmailAddress]
        public required string Email { get; set; } = null!;
        public Guid? PreviousProblemId { get; set; } = null;

    }
}
