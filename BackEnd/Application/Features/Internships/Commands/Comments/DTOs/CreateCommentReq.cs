using System.ComponentModel.DataAnnotations;

namespace Application.Features.Internships.Commands.Comments.DTOs
{
    public class CreateCommentReq
    {
        [Required]
        public int CommentTypeId { get; init; }
        [Required]
        public string Description { get; init; } = null!;
    }
}
