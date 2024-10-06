using System.ComponentModel.DataAnnotations;

namespace Application.Features.User.UserPart.DTOs.LoginIn
{
    public class LoginInRequestDto
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; } = null!;
        [Required]
        public required string Password { get; set; } = null!;
    }
}
