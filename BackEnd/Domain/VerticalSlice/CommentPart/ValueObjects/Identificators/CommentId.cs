using Domain.VerticalSlice.CommentPart.ValueObjects.CommentTypePart;
using Domain.VerticalSlice.IntershipPart.ValueObjects.Identificators;

namespace Domain.VerticalSlice.CommentPart.ValueObjects.Identificators
{
    public record CommentId
    {
        public IntershipId IntershipId { get; private set; }
        public CommentType CommentType { get; private set; }
        public DateTime Published { get; private set; }

        public CommentId
            (
            IntershipId intershipId,
            CommentType commentType,
            DateTime published
            )
        {
            IntershipId = intershipId;
            CommentType = commentType;
            Published = published;
        }
    }
}
