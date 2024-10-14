using Domain.Features.Comment.ValueObjects.CommentTypePart;
using Domain.Features.Intership.ValueObjects.Identificators;

namespace Domain.Features.Comment.ValueObjects.Identificators
{
    public record CommentId
    {
        //Values
        public IntershipId IntershipId { get; private set; }
        public CommentType CommentType { get; private set; }
        public DateTime Created { get; private set; }

        //Cosntructor
        public CommentId
            (
            IntershipId intershipId,
            CommentType commentType,
            DateTime created
            )
        {
            IntershipId = intershipId;
            CommentType = commentType;
            Created = created;
        }
    }
}
