using System.ComponentModel.DataAnnotations;

namespace Application.VerticalSlice.UserPart.DTOs.Refresh
{
    public class RefreshRequestDto
    {
        [Required]
        public required string RefreshToken { get; set; } = null!;
    }
}
