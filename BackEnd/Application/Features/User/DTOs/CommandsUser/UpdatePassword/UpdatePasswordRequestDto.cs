using System.ComponentModel.DataAnnotations;

namespace Application.Features.User.DTOs.CommandsUser.UpdatePassword
{
    public class UpdatePasswordRequestDto
    {
        [Required]
        public required string NewPassword { get; set; } = null!;
    }
}
