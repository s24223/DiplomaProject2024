using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Application.Features.Internships.Commands.Comments.DTOs
{
    public class CreateCommentEvaluationReq : CreateCommentReq
    {
        [JsonPropertyOrder(1)]
        [Required]
        public int Evaluation { get; set; }
    }
}
