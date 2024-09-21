using Domain.Providers;
using Domain.Templates.Entities;
using Domain.ValueObjects;
using Domain.ValueObjects.EntityIdentificators;
using Domain.ValueObjects.PartCommentType;

namespace Domain.Entities.RecrutmentPart
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
            IDomainProvider provider
            ) : base(new CommentId(
            new IntershipId(internshipId),
            new CommentType(commentTypeId),
            published
            ), provider)
        {
            Evaluation = (evaluation == null) ?
                null : new CommentEvaluation(evaluation.Value);

            Description = description;
        }
    }
}
