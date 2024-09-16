using System.ComponentModel.DataAnnotations;

namespace Application.VerticalSlice.UserPart.DTOs.Refresh
{
    public class RefreshRequestDto
    {
        [Required]
        public string RefreshToken { get; set; } = null!;
    }
}
