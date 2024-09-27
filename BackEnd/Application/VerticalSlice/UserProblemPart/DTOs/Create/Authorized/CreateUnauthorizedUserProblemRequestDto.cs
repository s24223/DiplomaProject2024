using System.ComponentModel.DataAnnotations;

namespace Application.VerticalSlice.UserProblemPart.DTOs.Create.Authorized
{
    public class CreateUnauthorizedUserProblemRequestDto
    {
        public required string UserMessage { get; set; } = null!;
        public Guid? PreviousProblemId { get; set; }
        [EmailAddress]
        public required string? Email { get; set; }
    }
}
