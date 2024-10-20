using System.ComponentModel.DataAnnotations;

namespace Application.Features.User.DTOs.CommandsUser.UpdateLogin
{
    public class UpdateLoginRequestDto
    {
        [Required]
        [EmailAddress]
        public required string NewLogin { get; set; } = null!;
    }
}
