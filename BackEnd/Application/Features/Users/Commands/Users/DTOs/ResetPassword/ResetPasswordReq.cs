using System.ComponentModel.DataAnnotations;

namespace Application.Features.Users.Commands.Users.DTOs.ResetPassword
{
    public class ResetPasswordReq
    {
        [Required]
        public string NewPassword { get; init; } = null!;
    }
}
