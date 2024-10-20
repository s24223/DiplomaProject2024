using System.ComponentModel.DataAnnotations;

namespace Application.Features.User.DTOs.CommandsUser.Refresh
{
    public class RefreshRequestDto
    {
        [Required]
        public required string RefreshToken { get; set; } = null!;
    }
}
