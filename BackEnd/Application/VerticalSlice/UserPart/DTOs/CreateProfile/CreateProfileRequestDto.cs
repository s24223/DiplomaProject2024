using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Application.VerticalSlice.UserPart.DTOs.CreateProfile
{
    public class CreateProfileRequestDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        [PasswordPropertyText]
        public string Password { get; set; } = null!;
    }
}
