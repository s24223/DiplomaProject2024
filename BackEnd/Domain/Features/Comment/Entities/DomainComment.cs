using Domain.Features.Comment.Exceptions.ValueObjects;
using Domain.Features.Comment.Reposoitories;
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
        private readonly ICommentTypeRepo _repo;
        //Values
        public string Description { get; private set; } = null!;
        public DomainCommentType Type { get; private set; } = null!;
        public CommentEvaluation? Evaluation { get; private set; } = null;


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
            CommentSenderEnum sender,
            CommentTypeEnum type,
            DateTime created,
            string description,
            int? evaluation,
            IProvider provider,
            ICommentTypeRepo repo
            ) : base(new CommentId(
            new RecrutmentId(internshipId),
                sender,
                type,
                created
            ), provider)
        {
            _repo = repo;

            if (Id.CommentTypeId < 1002)
            {
                if (evaluation == null)
                {
                    throw new CommentEvaluationException(Messages.Evaluation_Requered);
                }
                Evaluation = new CommentEvaluation(evaluation.Value);
            }
            Description = description;
            Type = _repo.GetValue(Id.CommentTypeId);
        }

        public DomainComment
            (
            Guid internshipId,
            int commentTypeId,
            DateTime created,
            string description,
            int? evaluation,
            IProvider provider,
            ICommentTypeRepo repo
            ) : base(new CommentId(
            new RecrutmentId(internshipId),
                commentTypeId,
                created
            ), provider)
        {
            _repo = repo;
            Description = description;
            Type = _repo.GetValue(Id.CommentTypeId);
        }
        //====================================================================================================
        //====================================================================================================
        //====================================================================================================
        //Public Methods
    }
}
