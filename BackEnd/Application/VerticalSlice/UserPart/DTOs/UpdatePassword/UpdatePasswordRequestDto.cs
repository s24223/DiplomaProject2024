using System.ComponentModel.DataAnnotations;

namespace Application.VerticalSlice.UserPart.DTOs.UpdatePassword
{
    public class UpdatePasswordRequestDto
    {
        [Required]
        public required string NewPassword { get; set; } = null!;
    }
}
