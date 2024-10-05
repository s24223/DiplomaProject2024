using Domain.Shared.Providers;
using Domain.Shared.Templates.Entities;
using Domain.Shared.ValueObjects;
using Domain.VerticalSlice.BranchOfferPart.Entities;
using Domain.VerticalSlice.BranchOfferPart.ValueObjects.Identificators;
using Domain.VerticalSlice.BranchPart.ValueObjects.Identificators;
using Domain.VerticalSlice.IntershipPart.Entities;
using Domain.VerticalSlice.OfferPart.ValueObjects.Identificators;
using Domain.VerticalSlice.PersonPart.Entities;
using Domain.VerticalSlice.RecruitmentPart.ValueObjects.Identificators;
using Domain.VerticalSlice.UserPart.ValueObjects.Identificators;

namespace Domain.VerticalSlice.RecruitmentPart.Entities
{
    public class DomainRecruitment : Entity<RecrutmentId>
    {
        //Values
        public DateTime ApplicationDate { get; private set; }
        public string? PersonMessage { get; private set; }
        public string? CompanyResponse { get; set; }
        public DatabaseBool? AcceptedRejected { get; set; }


        //References
        //DomainPerson
        private DomainPerson _person = null!;
        public DomainPerson Person
        {
            get { return _person; }
            set
            {
                if (_person == null && value != null && value.Id == Id.PersonId)
                {
                    _person = value;
                    _person.AddRecrutment(this);
                }
            }
        }
        //DomainBranchOffer
        private DomainBranchOffer _branchOffer = null!;
        public DomainBranchOffer BranchOffer
        {
            get { return _branchOffer; }
            set
            {
                if (_branchOffer == null && value != null && value.Id == Id.BranchOfferId)
                {
                    _branchOffer = value;
                    _branchOffer.AddRecrutment(this);
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
                if (_intership == null && value != null && value.RecrutmentId == Id)
                {
                    _intership = value;
                    _intership.Recrutment = this;
                }
            }
        }


        //Cosntructor
        public DomainRecruitment
            (
            Guid personId,
            Guid branchId,
            Guid offerId,
            DateTime created,
            DateTime? applicationDate,
            string? personMessage,
            string? companyResponse,
            string? acceptedRejected,
            IProvider provider
            ) : base(new RecrutmentId
                (
                new BranchOfferId(
                    new BranchId(branchId),
                    new OfferId(offerId),
                    created
                    ),
                new UserId(personId)
                ), provider)
        {

            PersonMessage = personMessage;
            CompanyResponse = companyResponse;

            ApplicationDate = applicationDate != null ?
                applicationDate.Value : _provider.TimeProvider().GetDateTimeNow();

            AcceptedRejected = string.IsNullOrWhiteSpace(acceptedRejected) ?
                null : new DatabaseBool(acceptedRejected);
        }
    }
}
