using System.ComponentModel.DataAnnotations;

namespace Application.VerticalSlice.UserPart.DTOs.CreateProfile
{
    public class CreateProfileRequestDto
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; } = null!;
        [Required]
        public required string Password { get; set; } = null!;
    }
}
