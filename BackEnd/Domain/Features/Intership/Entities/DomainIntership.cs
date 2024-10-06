using Domain.Features.Branch.ValueObjects.Identificators;
using Domain.Features.BranchOffer.ValueObjects.Identificators;
using Domain.Features.Comment.Entities;
using Domain.Features.Comment.ValueObjects.Identificators;
using Domain.Features.Intership.ValueObjects.Identificators;
using Domain.Features.Offer.ValueObjects.Identificators;
using Domain.Features.Recruitment.Entities;
using Domain.Features.Recruitment.ValueObjects.Identificators;
using Domain.Features.User.ValueObjects.Identificators;
using Domain.Shared.Providers;
using Domain.Shared.Templates.Entities;

namespace Domain.Features.Intership.Entities
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
