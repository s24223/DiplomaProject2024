using Domain.Features.Comment.ValueObjects;
using Domain.Features.Comment.ValueObjects.CommentTypePart;
using Domain.Features.Comment.ValueObjects.Identificators;
using Domain.Features.Intership.Entities;
using Domain.Features.Recruitment.ValueObjects.Identificators;
using Domain.Shared.Providers;
using Domain.Shared.Templates.Entities;

namespace Domain.Features.Comment.Entities
{
    public class DomainComment : Entity<CommentId>
    {
        //Values
        public string Description { get; private set; } = null!;
        public CommentEvaluation? Evaluation { get; private set; }


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
            DateTime created,
            string description,
            int? evaluation,
            IProvider provider
            ) : base(new CommentId(
            new RecrutmentId(internshipId),
            new DomainCommentType(commentTypeId),
            created
            ), provider)
        {
            Evaluation = evaluation == null ?
                null : new CommentEvaluation(evaluation.Value);

            Description = description;
        }


        //====================================================================================================
        //====================================================================================================
        //====================================================================================================
        //Public Methods
    }
}
