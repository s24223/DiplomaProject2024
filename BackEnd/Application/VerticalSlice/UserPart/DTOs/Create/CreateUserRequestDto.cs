using System.ComponentModel.DataAnnotations;

namespace Application.VerticalSlice.UserPart.DTOs.Create
{
    public class CreateUserRequestDto
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; } = null!;
        [Required]
        public required string Password { get; set; } = null!;
    }
}
