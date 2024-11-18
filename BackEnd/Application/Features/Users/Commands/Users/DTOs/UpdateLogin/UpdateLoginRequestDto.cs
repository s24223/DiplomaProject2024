using System.ComponentModel.DataAnnotations;

namespace Application.Features.Users.Commands.Users.DTOs.UpdateLogin
{
    public class UpdateLoginRequestDto
    {
        [Required]
        [EmailAddress]
        public required string NewLogin { get; set; } = null!;
    }
}
