using System.ComponentModel.DataAnnotations;

namespace Application.Features.Users.Commands.Users.DTOs
{
    public class DeleteProfileReq
    {
        [Required]
        public string Password { get; set; } = null!;
    }
}
