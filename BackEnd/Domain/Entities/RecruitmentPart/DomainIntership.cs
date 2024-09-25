using Domain.Providers;
using Domain.Templates.Entities;
using Domain.ValueObjects.EntityIdentificators;

namespace Domain.Entities.RecrutmentPart
{
    public class DomainIntership : Entity<IntershipId>
    {
        //Values
        public string ContractNumber { get; set; } = null!;


        //Refrences
        private Dictionary<CommentId, DomainComment> _comments = new();
        public IReadOnlyDictionary<CommentId, DomainComment> Comments => _comments;
        //Recrutment Refrences
        public RecrutmentId RecrutmentId { get; private set; }
        private DomainRecruitment _recrutment = null!;
        public DomainRecruitment Recrutment
        {
            get { return _recrutment; }
            set
            {
                if (_recrutment == null && value != null && value.Id == RecrutmentId)
                {
                    _recrutment = value;
                    _recrutment.Intership = this;
                }
            }
        }


        //Cosntructor
        public DomainIntership
            (
            Guid? id,
            Guid personId,
            Guid branchId,
            Guid offerId,
            DateTime created,
            string contractNumber,
            IDomainProvider provider
            ) : base(new IntershipId(id), provider)
        {
            ContractNumber = contractNumber;

            RecrutmentId = new RecrutmentId(
                new BranchOfferId(
                    new BranchId(branchId),
                    new OfferId(offerId),
                    created),
                new UserId(personId)
                );
        }


        //Methods
        public void AddComment(DomainComment domainComment)
        {
            if (domainComment.Id.IntershipId == this.Id && !_comments.ContainsKey(domainComment.Id))
            {
                _comments.Add(domainComment.Id, domainComment);
                domainComment.Intership = this;
            }
        }
    }
}
