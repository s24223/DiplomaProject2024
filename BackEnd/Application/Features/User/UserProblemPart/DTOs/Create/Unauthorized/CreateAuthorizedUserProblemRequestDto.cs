using System.ComponentModel.DataAnnotations;

namespace Application.Features.User.UserProblemPart.DTOs.Create.Unauthorized
{
    public class CreateAuthorizedUserProblemRequestDto
    {
        [Required]
        public required string UserMessage { get; set; } = null!;
        public Guid? PreviousProblemId { get; set; }
    }
}
