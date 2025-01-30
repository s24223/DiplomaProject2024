using Domain.Features.BranchOffer.Entities;
using Domain.Features.BranchOffer.ValueObjects.Identificators;
using Domain.Features.Intership.Entities;
using Domain.Features.Person.Entities;
using Domain.Features.Recruitment.ValueObjects.Identificators;
using Domain.Features.User.ValueObjects.Identificators;
using Domain.Shared.Providers;
using Domain.Shared.Templates.Entities;
using Domain.Shared.ValueObjects;

namespace Domain.Features.Recruitment.Entities
{
    public class DomainRecruitment : Entity<RecrutmentId>
    {
        //Values
        public DateTime Created { get; private set; }
        public string? PersonMessage { get; private set; }
        public string? CompanyResponse { get; private set; }
        public string? Url { get; set; } = null;
        public DatabaseBool? IsAccepted { get; private set; }


        //References
        //DomainPerson
        public UserId PersonId { get; private set; }
        private DomainPerson _person = null!;
        public DomainPerson Person
        {
            get { return _person; }
            set
            {
                if (_person == null && value != null && value.Id == PersonId)
                {
                    _person = value;
                    _person.SetRecrutment(this);
                }
            }
        }
        //DomainBranchOffer
        public BranchOfferId BranchOfferId { get; private set; }
        private DomainBranchOffer _branchOffer = null!;
        public DomainBranchOffer BranchOffer
        {
            get { return _branchOffer; }
            set
            {
                if (_branchOffer == null && value != null && value.Id == BranchOfferId)
                {
                    _branchOffer = value;
                    _branchOffer.AddRecrutments([this]);
                }
            }
        }
        //DomainIntership
        private DomainIntership? _intership = null;
        public DomainIntership? Intership
        {
            get { return _intership; }
            set
            {
                if (_intership == null && value != null && value.Id == Id)
                {
                    _intership = value;
                    _intership.Recrutment = this;
                }
            }
        }


        //Cosntructor
        public DomainRecruitment
            (
            Guid? id,
            Guid personId,
            Guid branchOfferId,
            DateTime? created,
            string? personMessage,
            string? companyResponse,
            string? isAccepted,
            IProvider provider
            ) : base(new RecrutmentId(id), provider)
        {
            PersonId = new UserId(personId);
            BranchOfferId = new BranchOfferId(branchOfferId);

            PersonMessage = personMessage;
            CompanyResponse = companyResponse;

            Created = created ?? _provider.TimeProvider().GetDateTimeNow();
            IsAccepted = string.IsNullOrWhiteSpace(isAccepted) ?
                null : new DatabaseBool(isAccepted);
        }


        //=====================================================================================================
        //=====================================================================================================
        //=====================================================================================================
        //Public Methods
        public void SetAnswer
            (
            string? companyResponse,
            bool isAccepted
            )
        {
            CompanyResponse = companyResponse;
            IsAccepted = new DatabaseBool(isAccepted);
        }

        //=====================================================================================================
        //=====================================================================================================
        //=====================================================================================================
        //Private Methods
    }
}
