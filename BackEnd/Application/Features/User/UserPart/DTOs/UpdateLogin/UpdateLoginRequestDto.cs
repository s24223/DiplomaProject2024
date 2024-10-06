using System.ComponentModel.DataAnnotations;

namespace Application.Features.User.UserPart.DTOs.UpdateLogin
{
    public class UpdateLoginRequestDto
    {
        [Required]
        [EmailAddress]
        public required string NewLogin { get; set; } = null!;
    }
}
