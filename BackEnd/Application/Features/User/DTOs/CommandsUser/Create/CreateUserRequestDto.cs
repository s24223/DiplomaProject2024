using System.ComponentModel.DataAnnotations;

namespace Application.Features.User.DTOs.CommandsUser.Create
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
