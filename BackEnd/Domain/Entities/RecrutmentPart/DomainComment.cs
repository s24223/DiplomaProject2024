using Domain.Providers;
using Domain.Templates.Entities;
using Domain.ValueObjects.EntityIdentificators;
using Domain.ValueObjects.PartCommentType;

namespace Domain.Entities.RecrutmentPart
{
    public class DomainComment : Entity<CommentId>
    {
        //Values
        public string Description { get; set; } = null!;
        public int? Evaluation { get; set; }


        //References
        public DomainIntership Intership { get; set; } = null!;


        //Constructor
        public DomainComment
            (
            Guid internshipId,
            int commentTypeId,
            DateTime published,
            string description,
            int? evaluation,
            IDomainProvider provider
            ) : base(new CommentId(
            new IntershipId(internshipId),
            new CommentType(commentTypeId),
            published
            ), provider)
        {
            Description = description;
            Evaluation = evaluation;
        }
    }
}
