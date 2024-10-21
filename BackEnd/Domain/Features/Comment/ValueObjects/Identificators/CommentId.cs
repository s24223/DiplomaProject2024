using Domain.Features.Comment.ValueObjects.CommentTypePart;
using Domain.Features.Recruitment.ValueObjects.Identificators;

namespace Domain.Features.Comment.ValueObjects.Identificators
{
    public record CommentId
    {
        //Values
        public RecrutmentId IntershipId { get; private set; }
        public DomainCommentType CommentType { get; private set; }
        public DateTime Created { get; private set; }

        //Cosntructor
        public CommentId
            (
            RecrutmentId intershipId,
            DomainCommentType commentType,
            DateTime created
            )
        {
            IntershipId = intershipId;
            CommentType = commentType;
            Created = created;
        }
    }
}
