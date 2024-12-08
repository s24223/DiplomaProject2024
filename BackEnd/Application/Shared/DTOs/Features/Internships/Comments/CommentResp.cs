using Domain.Features.Comment.Entities;

namespace Application.Shared.DTOs.Features.Internships.Comments
{
    public class CommentResp
    {
        public Guid InternshipId { get; set; }
        public int CommentTypeId { get; set; }
        public DateTime Created { get; set; }
        public string Description { get; set; } = null!;
        public int? Evaluation { get; set; }
        public CommentTypeResp Type { get; set; } = null!;


        public CommentResp(DomainComment domain)
        {
            InternshipId = domain.Id.IntershipId.Value;
            CommentTypeId = domain.Id.CommentTypeId;
            Created = domain.Id.Created;
            Description = domain.Description;
            Evaluation = domain.Evaluation?.Value;
            if (domain.Type != null)
            {
                Type = new CommentTypeResp(domain.Type);
            }
        }
    }
}
