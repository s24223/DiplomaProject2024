using System.ComponentModel.DataAnnotations;

namespace Application.Features.User.UserPart.DTOs.Refresh
{
    public class RefreshRequestDto
    {
        [Required]
        public required string RefreshToken { get; set; } = null!;
    }
}
