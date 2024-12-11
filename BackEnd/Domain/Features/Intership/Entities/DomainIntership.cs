using Domain.Features.Comment.Entities;
using Domain.Features.Comment.ValueObjects.Identificators;
using Domain.Features.Intership.ValueObjects;
using Domain.Features.Recruitment.Entities;
using Domain.Features.Recruitment.ValueObjects.Identificators;
using Domain.Shared.Providers;
using Domain.Shared.Templates.Entities;

namespace Domain.Features.Intership.Entities
{
    public class DomainIntership : Entity<RecrutmentId>
    {
        //Values
        public ContractNumber ContractNumber { get; private set; } = null!;
        public DateTime Created { get; private set; }
        public DateOnly ContractStartDate { get; private set; }
        public DateOnly? ContractEndDate { get; private set; }

        //Refrences
        //Comments 
        private Dictionary<CommentId, DomainComment> _comments = new();
        public IReadOnlyDictionary<CommentId, DomainComment> Comments => _comments;

        //Recrutment 
        private DomainRecruitment _recrutment = null!;
        public DomainRecruitment Recrutment
        {
            get { return _recrutment; }
            set
            {
                if (_recrutment == null && value != null && value.Id == Id)
                {
                    _recrutment = value;
                    _recrutment.Intership = this;
                }
            }
        }


        //Cosntructor
        public DomainIntership
            (
            Guid id,
            DateTime? created,
            DateOnly contractStartDate,
            DateOnly? contractEndDate,
            string contractNumber,
            IProvider provider
            ) : base(new RecrutmentId(id), provider)
        {
            ContractNumber = new ContractNumber(contractNumber);
            Created = created ?? _provider.TimeProvider().GetDateTimeNow();
            ContractStartDate = contractStartDate;
            ContractEndDate = contractEndDate;
        }


        //=====================================================================================================
        //=====================================================================================================
        //=====================================================================================================
        //Public Methods
        public void AddComments(IEnumerable<DomainComment> comments)
        {
            foreach (DomainComment comment in comments)
            {
                AddComment(comment);
            }
        }

        public void AddComment(DomainComment domainComment)
        {
            if (domainComment.Id.IntershipId == Id && !_comments.ContainsKey(domainComment.Id))
            {
                _comments.Add(domainComment.Id, domainComment);
                domainComment.Intership = this;
            }
        }

        public void Update
            (
            string contractNumber,
            DateOnly contractStartDate,
            DateOnly? contractEndDate
            )
        {
            ContractNumber = new ContractNumber(contractNumber);
            ContractStartDate = contractStartDate;
            ContractEndDate = contractEndDate;
        }
        //=====================================================================================================
        //=====================================================================================================
        //=====================================================================================================
        //Private Methods
    }
}
