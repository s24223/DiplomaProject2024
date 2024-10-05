using System.ComponentModel.DataAnnotations;

namespace Application.VerticalSlice.UserPart.DTOs.UpdateLogin
{
    public class UpdateLoginRequestDto
    {
        [Required]
        [EmailAddress]
        public required string NewLogin { get; set; } = null!;
    }
}
