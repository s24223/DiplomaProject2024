using Domain.Shared.Providers;
using Domain.Shared.Templates.Entities;
using Domain.VerticalSlice.CommentPart.ValueObjects;
using Domain.VerticalSlice.CommentPart.ValueObjects.CommentTypePart;
using Domain.VerticalSlice.CommentPart.ValueObjects.Identificators;
using Domain.VerticalSlice.IntershipPart.Entities;
using Domain.VerticalSlice.IntershipPart.ValueObjects.Identificators;

namespace Domain.VerticalSlice.CommentPart.Entities
{
    public class DomainComment : Entity<CommentId>
    {
        //Values
        public string Description { get; set; } = null!;
        public CommentEvaluation? Evaluation { get; set; }


        //References
        private DomainIntership _intership = null!;
        public DomainIntership Intership
        {
            get { return _intership; }
            set
            {
                if (_intership == null && value != null && value.Id == Id.IntershipId)
                {
                    _intership = value;
                    _intership.AddComment(this);
                }
            }
        }


        //Constructor
        public DomainComment
            (
            Guid internshipId,
            int commentTypeId,
            DateTime published,
            string description,
            int? evaluation,
            IProvider provider
            ) : base(new CommentId(
            new IntershipId(internshipId),
            new CommentType(commentTypeId),
            published
            ), provider)
        {
            Evaluation = evaluation == null ?
                null : new CommentEvaluation(evaluation.Value);

            Description = description;
        }
    }
}
