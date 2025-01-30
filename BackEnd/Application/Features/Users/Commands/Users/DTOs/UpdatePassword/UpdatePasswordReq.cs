using System.ComponentModel.DataAnnotations;

namespace Application.Features.Users.Commands.Users.DTOs.UpdatePassword
{
    public class UpdatePasswordReq
    {
        [Required]
        public required string OldPassword { get; init; } = null!;

        [Required]
        public required string NewPassword { get; init; } = null!;
    }
}
