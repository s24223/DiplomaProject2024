using System.ComponentModel.DataAnnotations;

namespace Application.Features.Users.Commands.Users.DTOs.UpdatePassword
{
    public class UpdatePasswordRequestDto
    {
        [Required]
        public required string NewPassword { get; set; } = null!;
    }
}
