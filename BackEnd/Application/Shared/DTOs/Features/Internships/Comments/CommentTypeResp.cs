using Domain.Features.Comment.ValueObjects.CommentTypePart;

namespace Application.Shared.DTOs.Features.Internships.Comments
{
    public class CommentTypeResp
    {
        //Values
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;


        //Cosntructor
        public CommentTypeResp(DomainCommentType domain)
        {
            Id = domain.Id;
            Name = domain.Name;
            Description = domain.Description;
        }
    }
}
