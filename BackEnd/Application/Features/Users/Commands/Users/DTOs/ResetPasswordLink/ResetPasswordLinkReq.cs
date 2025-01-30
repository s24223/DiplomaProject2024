using System.ComponentModel.DataAnnotations;

namespace Application.Features.Users.Commands.Users.DTOs.ResetPasswordLink
{
    public class ResetPasswordLinkReq
    {
        [Required]
        [EmailAddress]
        public string Email { get; init; } = null!;
    }
}
