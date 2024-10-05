using Domain.Shared.Providers;
using Domain.Shared.Templates.Entities;
using Domain.VerticalSlice.BranchOfferPart.ValueObjects.Identificators;
using Domain.VerticalSlice.BranchPart.ValueObjects.Identificators;
using Domain.VerticalSlice.CommentPart.Entities;
using Domain.VerticalSlice.CommentPart.ValueObjects.Identificators;
using Domain.VerticalSlice.IntershipPart.ValueObjects.Identificators;
using Domain.VerticalSlice.OfferPart.ValueObjects.Identificators;
using Domain.VerticalSlice.RecruitmentPart.Entities;
using Domain.VerticalSlice.RecruitmentPart.ValueObjects.Identificators;
using Domain.VerticalSlice.UserPart.ValueObjects.Identificators;

namespace Domain.VerticalSlice.IntershipPart.Entities
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
            IProvider provider
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
            if (domainComment.Id.IntershipId == Id && !_comments.ContainsKey(domainComment.Id))
            {
                _comments.Add(domainComment.Id, domainComment);
                domainComment.Intership = this;
            }
        }
    }
}
